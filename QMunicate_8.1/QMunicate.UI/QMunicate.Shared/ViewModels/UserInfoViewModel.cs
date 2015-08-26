using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Helper;
using QMunicate.Models;

namespace QMunicate.ViewModels 
{
    public class UserInfoViewModel : ViewModel
    {
        #region Fields

        private string userName;
        private ImageSource userImage;
        private string mobilePhone;
        private DialogVm dialog;

        #endregion

        #region Ctor

        public UserInfoViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessageCommandExecute, () => !IsLoading);
            DeleteHistoryCommand = new RelayCommand(DeleteHistoryCommandExecute, () => !IsLoading);
            RemoveContactCommand = new RelayCommand(RemoveContactCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public string UserName
        {
            get { return userName; }
            set { Set(ref userName, value); }
        }

        public ImageSource UserImage
        {
            get { return userImage; }
            set { Set(ref userImage, value); }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { Set(ref mobilePhone, value); }
        }

        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand DeleteHistoryCommand { get; set; }

        public RelayCommand RemoveContactCommand { get; set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dialogId = e.Parameter as string;
            if (!string.IsNullOrEmpty(dialogId))
            {
                var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
                dialog = dialogsManager.Dialogs.FirstOrDefault(d => d.Id == dialogId);
                if (dialog != null)
                {
                    int otherUserId = dialog.OccupantIds.FirstOrDefault(id => id != QuickbloxClient.CurrentUserId);
                    var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
                    var user = await cachingQbClient.GetUserById(otherUserId);
                    if (user != null)
                    {
                        UserName = user.FullName;
                        MobilePhone = user.Phone;
                        if (user.BlobId.HasValue)
                        {
                            var imageService = ServiceLocator.Locator.Get<IImageService>();
                            UserImage = await imageService.GetPrivateImage(user.BlobId.Value, 100);
                        }
                    }
                }
            }
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SendMessageCommand.RaiseCanExecuteChanged();
            DeleteHistoryCommand.RaiseCanExecuteChanged();
            RemoveContactCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private void SendMessageCommandExecute()
        {
            NavigationService.GoBack();
        }

        private async void DeleteHistoryCommandExecute()
        {
            IsLoading = true;
            if(await DeleteDialog())
                GoToDialogsPage();
            IsLoading = false;
        }

        private async  void RemoveContactCommandExecute()
        {
            IsLoading = true;
            int otherUserId = dialog.OccupantIds.FirstOrDefault(id => id != QuickbloxClient.CurrentUserId);
            var privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(otherUserId, dialog.Id);
            bool isDeleted = privateChatManager.DeleteFromFriends();

            if (isDeleted)
            {
                bool isDialogDeleted = await DeleteDialog();
                if (isDialogDeleted)
                {
                    GoToDialogsPage();
                }
            }
            IsLoading = false;
        }

        private async Task<bool> DeleteDialog()
        {
            var deleteResponse = await QuickbloxClient.ChatClient.DeleteDialogAsync(dialog.Id);
            if (deleteResponse.StatusCode == HttpStatusCode.OK)
            {
                var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
                var thisDialog = dialogsManager.Dialogs.FirstOrDefault(d => d.Id == dialog.Id);
                if (thisDialog != null) dialogsManager.Dialogs.Remove(thisDialog);
                return true;
            }

            return false;
        }

        private void GoToDialogsPage()
        {
            while (NavigationService.BackStack.Count > 1)
            {
                NavigationService.BackStack.RemoveAt(NavigationService.BackStack.Count - 1);
            }
            NavigationService.GoBack();
        }

        #endregion

    }
}
