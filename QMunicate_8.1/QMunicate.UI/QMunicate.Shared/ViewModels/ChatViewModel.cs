using QMunicate.Core.Command;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.Logger;
using QMunicate.Core.MessageService;
using QMunicate.Helper;
using QMunicate.Models;
using Quickblox.Sdk;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Quickblox.Sdk.GeneralDataModel.Models;
using Message = Quickblox.Sdk.GeneralDataModel.Models.Message;

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
        private int currentUserId;
        private IPrivateChatManager privateChatManager;
        private bool isMeTyping;
        private bool isOtherUserTyping;
        private readonly DispatcherTimer typingIndicatorTimer = new DispatcherTimer();
        private readonly TimeSpan typingIndicatorTimeout = new TimeSpan(0, 0, 10);
        private readonly DispatcherTimer pausedTypingTimer = new DispatcherTimer();
        private readonly TimeSpan pausedTypingTimeout = new TimeSpan(0, 0, 10);
        

        #endregion

        #region Ctor

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageVm>();
            SendCommand = new RelayCommand(SendCommandExecute, () => !IsLoading && IsMessageSendingAllowed);
            AcceptRequestCommand = new RelayCommand(AcceptRequestCommandExecute, () => !IsLoading);
            RejectRequestCommand = new RelayCommand(RejectCRequestCommandExecute, () => !IsLoading);
            ShowUserInfoCommand = new RelayCommand(ShowUserInfoCommandExecute, () => !IsLoading);
            typingIndicatorTimer.Interval = typingIndicatorTimeout;
            typingIndicatorTimer.Tick += (sender, o) => IsOtherUserTyping = false;
            pausedTypingTimer.Interval = pausedTypingTimeout;
            pausedTypingTimer.Tick += PausedTypingTimerOnTick;
        }

        #endregion

        #region Properties

        public ObservableCollection<MessageVm> Messages { get; set; }

        public string NewMessageText
        {
            get { return newMessageText; }
            set
            {
                Set(ref newMessageText, value);
                if(!string.IsNullOrEmpty(value)) NotifyIsTyping();
            }
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

        public bool IsOtherUserTyping
        {
            get { return isOtherUserTyping; }
            set { Set(ref isOtherUserTyping, value); }
        }

        public RelayCommand SendCommand { get; private set; }

        public RelayCommand AcceptRequestCommand { get; private set; }

        public RelayCommand RejectRequestCommand { get; private set; }

        public RelayCommand ShowUserInfoCommand { get; private set; }

        #endregion

        #region Navigation

        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var chatParameter = e.Parameter as ChatNavigationParameter;
            if (chatParameter == null) return;

            await Initialize(chatParameter);
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
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

            currentUserId = SettingsManager.Instance.ReadFromSettings<int>(SettingsKeys.CurrentUserId);

            if (chatParameter.Dialog != null)
            {
                dialog = chatParameter.Dialog;
                ChatName = chatParameter.Dialog.Name;
                ChatImage = chatParameter.Dialog.Image;

                int otherUserId = dialog.OccupantIds.FirstOrDefault(id => id != currentUserId);
                await QmunicateLoggerHolder.Log(QmunicateLogLevel.Debug, string.Format("Initializing Chat page. CurrentUserId: {0}. OtherUserId: {1}.", currentUserId, otherUserId));

                if (otherUserId != 0)
                {
                    privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(otherUserId, chatParameter.Dialog.Id);
                    privateChatManager.OnMessageReceived += ChatManagerOnOnMessageReceived;
                    privateChatManager.OnIsTyping += PrivateChatManagerOnOnIsTyping;
                    privateChatManager.OnPausedTyping += PrivateChatManagerOnOnPausedTyping;
                }
                if(!string.IsNullOrEmpty(chatParameter.Dialog.Id))
                    await LoadMessages(chatParameter.Dialog.Id);

                CheckIsMessageSendingAllowed();
            }

            IsLoading = false;
        }

        #region IsTyping functionality

        private void PrivateChatManagerOnOnIsTyping(object sender, EventArgs eventArgs)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                IsOtherUserTyping = true;

                typingIndicatorTimer.Stop();
                typingIndicatorTimer.Start();
            });
        }

        private void PrivateChatManagerOnOnPausedTyping(object sender, EventArgs eventArgs)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                IsOtherUserTyping = false;
            });
        }

        private void NotifyIsTyping()
        {
            if (privateChatManager == null || isMeTyping) return;

            pausedTypingTimer.Start();

            isMeTyping = true;
            privateChatManager.NotifyIsTyping();
        }

        private void PausedTypingTimerOnTick(object sender, object o)
        {
            NotifyPausedTyping();
        }

        private void NotifyPausedTyping()
        {
            pausedTypingTimer.Stop();
            if (privateChatManager == null) return;

            privateChatManager.NotifyPausedTyping();

            isMeTyping = false;
        }

        #endregion

        private void CheckIsMessageSendingAllowed()
        {
            for (int i = Messages.Count - 1; i >= 0; i--)
            {
                if (Messages[i].NotificationType == NotificationTypes.FriendsAccept)
                {
                    break;
                }

                if (Messages[i].NotificationType == NotificationTypes.FriendsReject)
                {
                    IsRequestRejected = true;
                    break;
                }

                if (Messages[i].MessageType == MessageType.Outgoing && Messages[i].NotificationType == NotificationTypes.FriendsRequest)
                {
                    IsWaitingForContactResponse = true;
                    break;
                }

                if (Messages[i].MessageType == MessageType.Incoming && Messages[i].NotificationType == NotificationTypes.FriendsRequest)
                {
                    IsActiveContactRequest = true;
                    break;
                }
            }
        }

        private async Task LoadMessages(string dialogId)
        {
            var response = await QuickbloxClient.ChatClient.GetMessagesAsync(dialogId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Messages.Clear();
                foreach (Message message in response.Result.Items)
                {
                    var msg = MessageVm.FromMessage(message, currentUserId);
                    Messages.Add(msg);
                }
            }
        }

        private async void SendCommandExecute()
        {
            if (string.IsNullOrWhiteSpace(NewMessageText)) return;

            NotifyPausedTyping();

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
            await dialogsManager.UpdateDialogLastMessage(dialog.Id, NewMessageText, DateTime.Now);

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
                CheckIsMessageSendingAllowed();
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
                CheckIsMessageSendingAllowed();
            }

            IsLoading = false;

        }

        private void ShowUserInfoCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.UserInfo, dialog == null ? null : dialog.Id);
        }

        private void ChatManagerOnOnMessageReceived(object sender, Message message)
        {
            var incomingMessage = new MessageVm
            {
                MessageText = message.MessageText,
                MessageType = MessageType.Incoming,
                DateTime = DateTime.Now
            };

            incomingMessage.NotificationType = message.NotificationType;

            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Messages.Add(incomingMessage);
                CheckIsMessageSendingAllowed();
            });
        }

        #endregion

    }
}
