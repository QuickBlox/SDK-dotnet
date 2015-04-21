using GalaSoft.MvvmLight.Views;
using Quickblox.Sdk;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using Quickblox.Sdk.Modules.MessagesModule;

namespace TestRequest.ViewModel
{
    public class ChatsViewModel : ViewModel, INavigatable
    {
        #region Fields

        private INavigationService navigationService;
        private QuickbloxClient quickbloxClient;
        private PrivateChatManager privateChatManager;
        private int otherUserId;
        private string messageText;
        private int userId;

        #endregion

        #region Ctor

        public ChatsViewModel(INavigationService navigationService, QuickbloxClient quickbloxClient)
        {
            this.navigationService = navigationService;
            this.quickbloxClient = quickbloxClient;
            Messages = new ObservableCollection<Message>();
            SendCommand = new RelayCommand(SendCommandExecute);
        }

        #endregion

        #region Properties

        public ObservableCollection<Message> Messages { get; set; }

        public int OtherUserId
        {
            get { return otherUserId; }
            set
            {
                if (otherUserId == value) return;
                otherUserId = value;
                RaisePropertyChanged(() => OtherUserId);
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                if (messageText == value) return;
                messageText = value;
                RaisePropertyChanged(() => MessageText);
            }
        }


        public RelayCommand SendCommand { get; set; }

        public int UserId
        {
            get { return userId; }
            set
            {
                if (userId == value) return;
                userId = value;
                RaisePropertyChanged(() => UserId);
            }
        }

        #endregion


        public void OnNavigated(object parameter)
        {
            UserId = (int) parameter;

        }

        private void PrivateChatManagerOnOnMessageReceived(object sender, Message msg)
        {
            Messages.Add(msg);
        }

        private void SendCommandExecute()
        {
            if (privateChatManager == null)
        {
                privateChatManager = quickbloxClient.MessagesClient.GetPrivateChatManager(OtherUserId);
                privateChatManager.OnMessageReceived += PrivateChatManagerOnOnMessageReceived;
            }
            

            if (string.IsNullOrEmpty(MessageText)) return;



            privateChatManager.SendMessage(MessageText);
        }
    }
}
