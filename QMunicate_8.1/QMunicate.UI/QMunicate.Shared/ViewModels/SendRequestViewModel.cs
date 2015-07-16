using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace QMunicate.ViewModels
{
    public class SendRequestViewModel : ViewModel
    {
        #region Fields

        private int otherUserId;
        private string userName;
        private ImageSource userImage;

        #endregion

        #region Ctor

        public SendRequestViewModel()
        {
            SendCommand = new RelayCommand(SendCommandExecute, () =>  !IsLoading);
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

        public ICommand SendCommand { get; private set; }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var user = e.Parameter as UserVm;
            if (user == null) return;

            UserImage = user.Image;
            UserName = user.FullName;
            otherUserId = user.UserId;
        }

        #endregion

        #region Private methods

        private async void SendCommandExecute()
        {
            IsLoading = true;
            var response = await QuickbloxClient.ChatClient.CreateDialogAsync(UserName, DialogType.Private, otherUserId.ToString());
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
                dialogsManager.Dialogs.Insert(0, DialogVm.FromDialog(response.Result));
                QuickbloxClient.MessagesClient.AddContact(new Contact(){Name = UserName, UserId = otherUserId});
                var privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(otherUserId);
                privateChatManager.SubsribeForPresence();

                var messagesService = ServiceLocator.Locator.Get<IMessageService>();
                await messagesService.ShowAsync("Sent", "A contact request was sent");
                IsLoading = false;
                NavigationService.GoBack();
            }
            IsLoading = false;
        }

        #endregion

    }
}
