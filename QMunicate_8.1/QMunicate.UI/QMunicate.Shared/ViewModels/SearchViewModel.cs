using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Responses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace QMunicate.ViewModels
{
    public class SearchViewModel : ViewModel
    {
        #region Fields

        private string searchText;
        private bool isInGlobalSeachMode;

        #endregion

        #region Ctor

        public SearchViewModel()
        {
            GlobalResults = new ObservableCollection<UserVm>();
            LocalResults = new ObservableCollection<UserVm>();
            OpenLocalCommand = new RelayCommand<UserVm>(u => OpenLocalCommandExecute(u));
            OpenGlobalCommand = new RelayCommand<UserVm>(OpenGlobalCommandExecute);
        }

        #endregion

        #region Properties

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (Set(ref searchText, value))
                    Search(searchText);
            }
        }

        public bool IsInGlobalSeachMode
        {
            get { return isInGlobalSeachMode; }
            set
            {
                if (Set(ref isInGlobalSeachMode, value))
                    Search(SearchText);
            }
        }

        public ObservableCollection<UserVm> GlobalResults { get; set; }

        public ObservableCollection<UserVm> LocalResults { get; set; }

        public RelayCommand<UserVm> OpenLocalCommand { get; set; }

        public RelayCommand<UserVm> OpenGlobalCommand { get; set; }

        #endregion

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LocalSearch("");
        }

        #region Private methods

        private async void Search(string searchQuery)
        {
            if(isInGlobalSeachMode)
                await GlobalSearch(searchQuery);
            else
                LocalSearch(searchQuery);
        }

        private async Task GlobalSearch(string searchQuery)
        {
            GlobalResults.Clear();
            if (string.IsNullOrWhiteSpace(searchQuery)) return;

            IsLoading = true;
            var response = await QuickbloxClient.UsersClient.GetUserByFullNameAsync(searchQuery, null, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                GlobalResults.Clear();
                foreach (UserResponse item in response.Result.Items)
                {
                    GlobalResults.Add(UserVm.FromUser(item.User));
                }
            }

            IsLoading = false;
        }

        private void LocalSearch(string searchQuery)
        {
            LocalResults.Clear();
            if (string.IsNullOrEmpty(searchQuery))
            {
                foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
                {
                    LocalResults.Add(UserVm.FromContact(contact));
                }
            }
            else
            {
                foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    LocalResults.Add(UserVm.FromContact(contact));
                }
            }
        }

        private async Task OpenLocalCommandExecute(UserVm user)
        {
            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            if (!dialogsManager.Dialogs.Any()) await dialogsManager.ReloadDialogs();
            var userDialog = dialogsManager.Dialogs.FirstOrDefault(d => d.Type == DialogType.Private && d.OccupantsIds.Contains(user.UserId));
            if(userDialog != null)
                NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter { Dialog = DialogVm.FromDialog(userDialog) });
        }

        private void OpenGlobalCommandExecute(UserVm user)
        {
            NavigationService.Navigate(ViewLocator.SendRequest);
        }

        #endregion

    }
}
