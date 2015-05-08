using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using QMunicate.Core.Command;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;

namespace QMunicate.ViewModels
{
    public class ChatsViewModel : ViewModel
    {
        #region Fields
        
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

        public ChatsViewModel()
        {
            this.Messages = new ObservableCollection<Message>();
            this.SendCommand = new RelayCommand(this.SendCommandExecute);
            this.GroupSendCommand = new RelayCommand(this.GroupSendCommandExecute);
            this.CreateDialogCommand = new RelayCommand(this.CreateDialogCommandExecute);
            this.GroupSendPresenceCommand = new RelayCommand(this.GroupSendPresenceCommandExecute);
            this.PresenceSubscribeCommand = new RelayCommand(this.PresenceSubscribeCommandExecute);
            this.ApproveSubsCommand = new RelayCommand(this.ApproveSubsCommandExecute);
            this.GetRosterCommand = new RelayCommand(this.GetRosterComandExecute);
        }

        private void GetRosterComandExecute()
        {
            this.QuickbloxClient.MessagesClient.DeleteContact(2766516);
        }

        #endregion

        #region Properties

        public ObservableCollection<Message> Messages { get; set; }

        public int UserId
        {
            get { return this.userId; }
            set { this.Set(ref this.userId, value); }
        }

        public int OtherUserId
        {
            get { return this.otherUserId; }
            set
            {
                this.Set(ref this.otherUserId, value);
                this.otherUserIdChanged = true;
            }
        }

        public string GroupJid
        {
            get { return this.groupJid; }
            set
            {
                this.Set(ref this.groupJid, value);
                this.groupJidChanged = true;
            }
        }

        public string MessageText
        {
            get { return this.messageText; }
            set { this.Set(ref this.messageText, value); }
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
            this.UserId = (int) parameter;
            this.QuickbloxClient.MessagesClient.OnMessageReceived += this.PrivateChatManagerOnOnMessageReceived;
        }


        private void PrivateChatManagerOnOnMessageReceived(object sender, Message msg)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this.Messages.Add(msg);
                });
        }

        private void SendCommandExecute()
        {
            if (this.privateChatManager == null || this.otherUserIdChanged)
            {
                this.privateChatManager = this.QuickbloxClient.MessagesClient.GetPrivateChatManager(this.OtherUserId);
                this.otherUserIdChanged = false;
            }
            
            if (string.IsNullOrEmpty(this.MessageText)) return;

            this.privateChatManager.SendMessage(this.MessageText);
        }

        private void GroupSendCommandExecute()
        {
            if (this.groupChatManager == null || this.groupJidChanged)
            {
                this.groupChatManager = this.QuickbloxClient.MessagesClient.GetGroupChatManager(this.GroupJid);
                this.groupJidChanged = false;
            }

            if (string.IsNullOrEmpty(this.MessageText)) return;

            this.groupChatManager.SendMessage(this.MessageText);
        }

        private void PresenceSubscribeCommandExecute()
        {
            if (this.privateChatManager == null || this.otherUserIdChanged)
            {
                this.privateChatManager = this.QuickbloxClient.MessagesClient.GetPrivateChatManager(this.OtherUserId);
                this.otherUserIdChanged = false;
            }

            this.privateChatManager.SubsribeForPresence();
        }

        private void ApproveSubsCommandExecute()
        {
            if (this.privateChatManager == null || this.otherUserIdChanged)
            {
                this.privateChatManager = this.QuickbloxClient.MessagesClient.GetPrivateChatManager(this.OtherUserId);
                this.otherUserIdChanged = false;
            }

            this.privateChatManager.ApproveSubscribtionRequest();
        }

        private void GroupSendPresenceCommandExecute()
        {
            if (this.groupChatManager == null || this.groupJidChanged)
            {
                this.groupChatManager = this.QuickbloxClient.MessagesClient.GetGroupChatManager(this.GroupJid);
                this.groupJidChanged = false;
            }

            Random random = new Random();
            string nick = "nick" + random.Next(10000);

            this.groupChatManager.JoinGroup(nick);
        }

        private async void CreateDialogCommandExecute()
        {
            var response = await this.QuickbloxClient.ChatClient.CreateDialogAsync("Testgroup", DialogType.PublicGroup);
        }


    }
}
