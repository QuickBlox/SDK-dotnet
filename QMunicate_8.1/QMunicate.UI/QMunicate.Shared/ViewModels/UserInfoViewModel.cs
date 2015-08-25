using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            MobilePhone = "38095461613";
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
                        //MobilePhone = user.Phone;
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

        private void DeleteHistoryCommandExecute()
        {

        }

        private void RemoveContactCommandExecute()
        {

        }

        #endregion

    }
}
