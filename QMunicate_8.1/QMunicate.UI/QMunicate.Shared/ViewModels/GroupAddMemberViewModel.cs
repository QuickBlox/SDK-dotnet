using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace QMunicate.ViewModels
{
    public class GroupAddMemberViewModel : ViewModel
    {
        private string searchText;
        private string membersText;

        #region Ctor

         public GroupAddMemberViewModel()
        {
            Contacts = new ObservableCollection<UserVm>();
            CreateGroupCommand = new RelayCommand(CreateGroupCommandExecute);
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

        public string MembersText
        {
            get { return membersText; }
            set { Set(ref membersText, value); }
        }

        public ObservableCollection<UserVm> Contacts { get; set; }

        public RelayCommand CreateGroupCommand { get; set; }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
            {
                Contacts.Add(UserVm.FromContact(contact));
            }

            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (UserVm userVm in Contacts)
            {
                var userResponse = await QuickbloxClient.UsersClient.GetUserByIdAsync(userVm.UserId);
                if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result.User.BlobId.HasValue)
                {
                    userVm.Image = await imagesService.GetPrivateImage(userResponse.Result.User.BlobId.Value);
                }
            }
        }

        #endregion

        private async void Search(string searchQuery)
        {
            Contacts.Clear();
            if (string.IsNullOrEmpty(searchQuery))
            {
                foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts)
                {
                    Contacts.Add(UserVm.FromContact(contact));
                }
            }
            else
            {
                foreach (Contact contact in QuickbloxClient.MessagesClient.Contacts.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    Contacts.Add(UserVm.FromContact(contact));
                }
            }

            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (UserVm userVm in Contacts)
            {
                var userResponse = await QuickbloxClient.UsersClient.GetUserByIdAsync(userVm.UserId);
                if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result.User.BlobId.HasValue)
                {
                    userVm.Image = await imagesService.GetPrivateImage(userResponse.Result.User.BlobId.Value);
                }
            }
        }

        private void CreateGroupCommandExecute()
        {
            
        }
    }
}
