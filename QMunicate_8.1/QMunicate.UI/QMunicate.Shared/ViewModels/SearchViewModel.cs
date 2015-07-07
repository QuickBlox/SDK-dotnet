using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Responses;

namespace QMunicate.ViewModels
{
    public class SearchViewModel : ViewModel
    {
        #region Fields

        private string searchText;

        #endregion

        #region Ctor

        public SearchViewModel()
        {
            GlobalResults = new ObservableCollection<UserVm>();
            LocalResults = new ObservableCollection<UserVm>();
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

        public ObservableCollection<UserVm> GlobalResults { get; set; }

        public ObservableCollection<UserVm> LocalResults { get; set; }

        #endregion

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadLocalUsers();
        }

        #region Private methods

        private async void Search(string searchQuery)
        {
            IsLoading = true;
            await GlobalSearch(searchQuery);
            LocalSearch(searchQuery);
            IsLoading = false;
        }

        private async Task GlobalSearch(string searchQuery)
        {
            GlobalResults.Clear();
            if (string.IsNullOrWhiteSpace(searchQuery)) return;

            var response = await QuickbloxClient.UsersClient.GetUserByFullNameAsync(searchQuery, null, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                GlobalResults.Clear();
                foreach (UserResponse item in response.Result.Items)
                {
                    GlobalResults.Add(UserVm.FromUser(item.User));
                }
            }
        }

        private void LocalSearch(string searchQuery)
        {

        }

        private async void LoadLocalUsers()
        {
            QuickbloxClient.MessagesClient.OnContactsChanged += MessagesClientOnOnContactsChanged;
            QuickbloxClient.MessagesClient.ReloadContacts();
        }

        private void MessagesClientOnOnContactsChanged(object sender, EventArgs eventArgs)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LocalResults.Clear());
            
            if (QuickbloxClient.MessagesClient.Contacts != null)
            {
                foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
                {
                    var contact1 = contact;
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LocalResults.Add(UserVm.FromContact(contact1)));
                }
            }
        }

        #endregion

    }
}
