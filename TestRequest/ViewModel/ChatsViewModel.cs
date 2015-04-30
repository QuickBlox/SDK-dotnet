using GalaSoft.MvvmLight.Views;
using Quickblox.Sdk;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GalaSoft.MvvmLight.Command;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;

namespace TestRequest.ViewModel
{
    public class ChatsViewModel : ViewModel, INavigatable
    {
        #region Fields

        private INavigationService navigationService;
        private QuickbloxClient quickbloxClient;

        private IPrivateChatManager privateChatManager;
        private IGroupChatManager groupChatManager;

        private int userId;
        private int otherUserId;
        private string groupJid;
        private string messageText;

        private bool otherUserIdChanged;
        private bool groupJidChanged;

        #endregion

        #region Ctor

        public ChatsViewModel(INavigationService navigationService, QuickbloxClient quickbloxClient)
        {
            this.navigationService = navigationService;
            this.quickbloxClient = quickbloxClient;
            Messages = new ObservableCollection<Message>();
            SendCommand = new RelayCommand(SendCommandExecute);
            GroupSendCommand = new RelayCommand(GroupSendCommandExecute);
            CreateDialogCommand = new RelayCommand(CreateDialogCommandExecute);
            GroupSendPresenceCommand = new RelayCommand(GroupSendPresenceCommandExecute);
            PresenceSubscribeCommand = new RelayCommand(PresenceSubscribeCommandExecute);
            ApproveSubsCommand = new RelayCommand(ApproveSubsCommandExecute);
            GetRosterCommand = new RelayCommand(GetRosterComandExecute);
        }

        private void GetRosterComandExecute()
        {
            quickbloxClient.MessagesClient.DeleteContact(2766516);
        }

        #endregion

        #region Properties

        public ObservableCollection<Message> Messages { get; set; }

        public int UserId
        {
            get { return userId; }
            set { Set(ref userId, value); }
        }

        public int OtherUserId
        {
            get { return otherUserId; }
            set
            {
                Set(ref otherUserId, value);
                otherUserIdChanged = true;
            }
        }

        public string GroupJid
        {
            get { return groupJid; }
            set
            {
                Set(ref groupJid, value);
                groupJidChanged = true;
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set { Set(ref messageText, value); }
        }

        public RelayCommand SendCommand { get; set; }

        public RelayCommand PresenceSubscribeCommand { get; set; }

        public RelayCommand ApproveSubsCommand { get; set; }

        public RelayCommand GroupSendCommand { get; set; }

        public RelayCommand GroupSendPresenceCommand { get; set; }

        public RelayCommand CreateDialogCommand { get; set; }

        public RelayCommand GetRosterCommand { get; set; }

        #endregion


        public void OnNavigated(object parameter)
        {
            UserId = (int) parameter;
            quickbloxClient.MessagesClient.OnMessageReceived += PrivateChatManagerOnOnMessageReceived;
        }


        private void PrivateChatManagerOnOnMessageReceived(object sender, Message msg)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    Messages.Add(msg);
                });
        }

        private void SendCommandExecute()
        {
            if (privateChatManager == null || otherUserIdChanged)
            {
                privateChatManager = quickbloxClient.MessagesClient.GetPrivateChatManager(OtherUserId);
                otherUserIdChanged = false;
            }
            
            if (string.IsNullOrEmpty(MessageText)) return;

            privateChatManager.SendMessage(MessageText, null);
        }

        private void GroupSendCommandExecute()
        {
            if (groupChatManager == null || groupJidChanged)
            {
                groupChatManager = quickbloxClient.MessagesClient.GetGroupChatManager(GroupJid);
                groupJidChanged = false;
            }

            if (string.IsNullOrEmpty(MessageText)) return;

            groupChatManager.SendMessage(MessageText);
        }

        private void PresenceSubscribeCommandExecute()
        {
            if (privateChatManager == null || otherUserIdChanged)
            {
                privateChatManager = quickbloxClient.MessagesClient.GetPrivateChatManager(OtherUserId);
                otherUserIdChanged = false;
            }

            privateChatManager.SubsribeForPresence();
        }

        private void ApproveSubsCommandExecute()
        {
            if (privateChatManager == null || otherUserIdChanged)
            {
                privateChatManager = quickbloxClient.MessagesClient.GetPrivateChatManager(OtherUserId);
                otherUserIdChanged = false;
            }

            privateChatManager.ApproveSubscribtionRequest();
        }

        private void GroupSendPresenceCommandExecute()
        {
            if (groupChatManager == null || groupJidChanged)
            {
                groupChatManager = quickbloxClient.MessagesClient.GetGroupChatManager(GroupJid);
                groupJidChanged = false;
            }

            Random random = new Random();
            string nick = "nick" + random.Next(10000);

            groupChatManager.JoinGroup(nick);
        }

        private async void CreateDialogCommandExecute()
        {
            var response = await quickbloxClient.ChatClient.CreateDialog("Testgroup", DialogType.PublicGroup);
        }


    }
}
