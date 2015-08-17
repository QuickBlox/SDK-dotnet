using QMunicate.Core.Command;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using Quickblox.Logger;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Message = Quickblox.Sdk.Modules.ChatModule.Models.Message;

namespace QMunicate.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        #region Fields

        private string newMessageText;
        private string chatName;
        private ImageSource chatImage;
        private bool isActiveContactRequest;
        private bool isWaitingForContactResponse;
        private bool isRequestRejected;
        private DialogVm dialog;
        private IPrivateChatManager privateChatManager;

        #endregion

        #region Ctor

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageVm>();
            SendCommand = new RelayCommand(SendCommandExecute, () => !IsLoading && IsMessageSendingAllowed);
            AcceptRequestCommand = new RelayCommand(AcceptRequestCommandExecute, () => !IsLoading);
            RejectRequestCommand = new RelayCommand(RejectCRequestCommandExecute, () => !IsLoading);
        }

        #endregion

        #region Properties

        public ObservableCollection<MessageVm> Messages { get; set; }

        public string NewMessageText
        {
            get { return newMessageText; }
            set { Set(ref newMessageText, value); }
        }

        public string ChatName
        {
            get { return chatName; }
            set { Set(ref chatName, value); }
        }

        public ImageSource ChatImage
        {
            get { return chatImage; }
            set { Set(ref chatImage, value); }
        }

        public bool IsActiveContactRequest
        {
            get { return isActiveContactRequest; }
            set
            {
                Set(ref isActiveContactRequest, value);
                RaisePropertyChanged(()=> IsMessageSendingAllowed);
            }
        }

        public bool IsWaitingForContactResponse
        {
            get { return isWaitingForContactResponse; }
            set
            {
                Set(ref isWaitingForContactResponse, value);
                RaisePropertyChanged(() => IsMessageSendingAllowed);
                
            }
        }

        public bool IsRequestRejected
        {
            get { return isRequestRejected; }
            set
            {
                Set(ref isRequestRejected, value);
                RaisePropertyChanged(() => IsMessageSendingAllowed);                
            }
        }

        public bool IsMessageSendingAllowed
        {
            get { return !IsActiveContactRequest && !IsRequestRejected && !IsWaitingForContactResponse; }
        }

        public RelayCommand SendCommand { get; private set; }

        public RelayCommand AcceptRequestCommand { get; private set; }

        public RelayCommand RejectRequestCommand { get; private set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var chatParameter = e.Parameter as ChatNavigationParameter;
            if (chatParameter == null) return;

            await Initialize(chatParameter);
        }

        public override void OnNavigatedFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (privateChatManager != null) privateChatManager.OnMessageReceived -= ChatManagerOnOnMessageReceived;
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SendCommand.RaiseCanExecuteChanged();
            
        }

        #endregion

        #region Private methods

        private async Task Initialize(ChatNavigationParameter chatParameter)
        {
            IsLoading = true;

            if (chatParameter.Dialog != null)
            {
                dialog = chatParameter.Dialog;
                ChatName = chatParameter.Dialog.Name;
                ChatImage = chatParameter.Dialog.Image;

                int otherUserId = dialog.OccupantIds.FirstOrDefault(id => id != QuickbloxClient.CurrentUserId);
                await FileLogger.Instance.Log(LogLevel.Debug, string.Format("Initializing Chat page. CurrentUserId: {0}. OtherUserId: {1}.", QuickbloxClient.CurrentUserId, otherUserId));

                if (otherUserId != 0)
                {
                    privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(otherUserId, chatParameter.Dialog.Id);
                    privateChatManager.OnMessageReceived += ChatManagerOnOnMessageReceived;
                }
                if(!string.IsNullOrEmpty(chatParameter.Dialog.Id))
                    await LoadMessages(chatParameter.Dialog.Id);

                for (int i = Messages.Count - 1; i >= 0; i--)
                {
                    if (Messages[i].NotificationType == NotificationTypes.FriendsAccept)
                    {
                        break;
                    }

                    if (Messages[i].MessageType == MessageType.Incoming && Messages[i].NotificationType == NotificationTypes.FriendsRequest)
                    {
                        IsActiveContactRequest = true;
                        break;
                    }

                    if (Messages[i].MessageType == MessageType.Outgoing && Messages[i].NotificationType == NotificationTypes.FriendsRequest)
                    {
                        IsWaitingForContactResponse = true;
                        break;
                    }

                    if (Messages[i].MessageType == MessageType.Incoming && Messages[i].NotificationType == NotificationTypes.FriendsReject)
                    {
                        IsRequestRejected = true;
                        break;
                    }
                }

                if (Messages.Any(m => m.MessageType == MessageType.Incoming && m.NotificationType == NotificationTypes.FriendsRequest)
                    && !Messages.Any(m => m.MessageType == MessageType.Outgoing && (m.NotificationType == NotificationTypes.FriendsAccept || m.NotificationType == NotificationTypes.FriendsReject)))
                    IsActiveContactRequest = true;
            }

            IsLoading = false;
        }

        private async Task LoadMessages(string dialogId)
        {
            var response = await QuickbloxClient.ChatClient.GetMessagesAsync(dialogId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Messages.Clear();
                foreach (Message message in response.Result.Items)
                {
                    var msg = MessageVm.FromMessage(message, QuickbloxClient.CurrentUserId);
                    Messages.Add(msg);
                }
            }
        }

        private async void SendCommandExecute()
        {
            if (string.IsNullOrWhiteSpace(NewMessageText)) return;

            //TODO: check is not needed. Navigate to proper page when opening dialog or do not show public dialogs
            if (dialog.DialogType != DialogType.Private)
            {
                var messageService = ServiceLocator.Locator.Get<IMessageService>();
                await messageService.ShowAsync("Message", "This is a public group dialog. You cannot send messages to public groups yet.");
                return;
            }

            bool isMessageSent = privateChatManager.SendMessage(NewMessageText);

            if (!isMessageSent)
            {
                var messageService = ServiceLocator.Locator.Get<IMessageService>();
                await messageService.ShowAsync("Message", "Failed to send a message");
                return;
            }

            var msg = new MessageVm()
            {
                MessageText = NewMessageText,
                MessageType = MessageType.Outgoing,
                DateTime = DateTime.Now
            };

            Messages.Add(msg);
            var dialogsManager = ServiceLocator.Locator.Get<IDialogsManager>();
            await dialogsManager.UpdateDialog(dialog.Id, NewMessageText, DateTime.Now);

            NewMessageText = "";
        }

        private async void AcceptRequestCommandExecute()
        {
            if (privateChatManager == null) return;
            IsLoading = true;
            bool accepted = await privateChatManager.AcceptFriend();

            if (accepted)
            {
                IsActiveContactRequest = false;
                await LoadMessages(dialog.Id);
            }
            

            IsLoading = false;
        }

        private async void RejectCRequestCommandExecute()
        {
            if (privateChatManager == null) return;

            IsLoading = true;
            bool rejected = await privateChatManager.RejectFriend();

            if (rejected)
            {
                IsActiveContactRequest = false;
                await LoadMessages(dialog.Id);
            }

            IsLoading = false;

        }

        private void ChatManagerOnOnMessageReceived(object sender, Quickblox.Sdk.Modules.MessagesModule.Models.Message message)
        {
            var incomingMessage = new MessageVm
            {
                MessageText = message.MessageText,
                MessageType = MessageType.Incoming,
                DateTime = DateTime.Now
            };

            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Messages.Add(incomingMessage));
        }

        #endregion

    }
}
