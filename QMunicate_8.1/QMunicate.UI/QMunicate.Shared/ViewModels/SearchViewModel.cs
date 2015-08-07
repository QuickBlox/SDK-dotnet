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
using Nito.AsyncEx;

namespace QMunicate.ViewModels
{
    public class SearchViewModel : ViewModel
    {
        #region Fields

        private string searchText;
        private bool isInGlobalSeachMode;
        private readonly AsyncLock localResultsLock = new AsyncLock();
        private readonly AsyncLock globalResultsLock = new AsyncLock();

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

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await LocalSearch("");
        }

        #endregion

        #region Private methods

        private async void Search(string searchQuery)
        {
            if(isInGlobalSeachMode)
                await GlobalSearch(searchQuery);
            else
                await LocalSearch(searchQuery);
        }

        private async Task GlobalSearch(string searchQuery)
        {
            GlobalResults.Clear();
            if (string.IsNullOrWhiteSpace(searchQuery)) return;

            IsLoading = true;
            var response = await QuickbloxClient.UsersClient.GetUserByFullNameAsync(searchQuery, null, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (await globalResultsLock.LockAsync())
                {
                    GlobalResults.Clear();
                    foreach (UserResponse item in response.Result.Items)
                    {
                        GlobalResults.Add(UserVm.FromUser(item.User));
                    }

                    foreach (UserVm userVm in GlobalResults)
                    {
                        if (userVm.ImageUploadId.HasValue)
                        {
                            var imagesService = ServiceLocator.Locator.Get<IImageService>();
                            userVm.Image = await imagesService.GetPrivateImage(userVm.ImageUploadId.Value);
                        }
                    }
                }
            }

            IsLoading = false;
        }

        private async Task LocalSearch(string searchQuery)
        {
            using (await localResultsLock.LockAsync())
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

                var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
                var imagesService = ServiceLocator.Locator.Get<IImageService>();
                foreach (UserVm userVm in LocalResults)
                {
                    var user = await cachingQbClient.GetUserById(userVm.UserId);
                    if (user != null && user.BlobId.HasValue)
                    {
                        userVm.Image = await imagesService.GetPrivateImage(user.BlobId.Value);
                    }
                }
            }
        }

        private async Task OpenLocalCommandExecute(UserVm user)
        {
            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            if (!dialogsManager.Dialogs.Any()) await dialogsManager.ReloadDialogs();
            var userDialog = dialogsManager.Dialogs.FirstOrDefault(d => d.DialogType == DialogType.Private && d.OccupantIds.Contains(user.UserId));
            if(userDialog != null)
                NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter { Dialog = userDialog });
            else
            {
                //TODO: review this
                var response = await QuickbloxClient.ChatClient.CreateDialogAsync(user.FullName, DialogType.Private, string.Format("{0}", user.UserId));
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    dialogsManager.Dialogs.Add(DialogVm.FromDialog(response.Result));
                    NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter { Dialog = DialogVm.FromDialog(response.Result) });
                }
            }
        }

        private void OpenGlobalCommandExecute(UserVm user)
        {
            NavigationService.Navigate(ViewLocator.SendRequest, user);
        }

        #endregion

    }
}
