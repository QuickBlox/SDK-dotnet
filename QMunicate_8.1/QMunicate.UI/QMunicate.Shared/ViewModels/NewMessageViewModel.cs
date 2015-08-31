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
    public class NewMessageViewModel : ViewModel
    {
        private string searchText;

        #region Ctor

        public NewMessageViewModel()
        {
            Contacts = new ObservableCollection<UserVm>();
            CreateGroupCommand = new RelayCommand(CreateGroupCommandExecute, () => !IsLoading);
            OpenContactCommand = new RelayCommand<UserVm>(u => OpenContactCommandExecute(u), u => !IsLoading);
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

        public ObservableCollection<UserVm> Contacts { get; set; }

        public RelayCommand CreateGroupCommand { get; set; }

        public RelayCommand<UserVm> OpenContactCommand { get; set; }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Search(null);
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            CreateGroupCommand.RaiseCanExecuteChanged();
            OpenContactCommand.RaiseCanExecuteChanged();
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

            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var imagesService = ServiceLocator.Locator.Get<IImageService>();
            foreach (UserVm userVm in Contacts)
            {
                var user = await cachingQbClient.GetUserById(userVm.UserId);
                if (user != null && user.BlobId.HasValue)
                {
                    userVm.ImageUploadId = user.BlobId;
                    userVm.Image = await imagesService.GetPrivateImage(user.BlobId.Value, 100, 100);
                }
            }
        }

        private void CreateGroupCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.GroupAddMember);
        }


        private async Task OpenContactCommandExecute(UserVm user)
        {
            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            if (!dialogsManager.Dialogs.Any()) await dialogsManager.ReloadDialogs();
            var userDialog = dialogsManager.Dialogs.FirstOrDefault(d => d.DialogType == DialogType.Private && d.OccupantIds.Contains(user.UserId));
            if (userDialog != null)
                NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter { Dialog = userDialog });
            else
            {
                var response = await QuickbloxClient.ChatClient.CreateDialogAsync(user.FullName, DialogType.Private, user.UserId.ToString());
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var dialogVm = DialogVm.FromDialog(response.Result);
                    dialogVm.Image = user.Image;
                    dialogVm.PrivatePhotoId = user.ImageUploadId;
                    dialogVm.Name = user.FullName;
                    dialogsManager.Dialogs.Insert(0, dialogVm);
                    NavigationService.Navigate(ViewLocator.Chat, new ChatNavigationParameter { Dialog = dialogVm });
                }
            }
        }

    }
}
