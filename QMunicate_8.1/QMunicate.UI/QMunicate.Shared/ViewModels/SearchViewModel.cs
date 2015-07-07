using QMunicate.Models;
using Quickblox.Sdk;
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

        #endregion

    }
}
