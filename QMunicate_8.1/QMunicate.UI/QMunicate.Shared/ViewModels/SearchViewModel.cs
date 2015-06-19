using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using QMunicate.Models;
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

        #endregion

        #region Private methods

        private async void Search(string searchQuery)
        {
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
            else
            {
                GlobalResults.Clear();
            }

            IsLoading = false;
        }

        #endregion

    }
}
