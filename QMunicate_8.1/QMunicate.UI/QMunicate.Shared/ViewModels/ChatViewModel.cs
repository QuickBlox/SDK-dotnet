using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.Command;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;

namespace QMunicate.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        #region Fields

        private int curentUserId;
        private string newMessageText;
        private string chatName;
        private string chatImage;
        private DialogVm dialog;

        #endregion

        #region Ctor

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageVm>();
            SendCommand = new RelayCommand(SendCommandExecute);
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

        public ICommand SendCommand { get; set; }

        public string ChatImage
        {
            get { return chatImage; }
            set { Set(ref chatImage, value); }
        }

        #endregion

        #region Navigation

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            var chatParameter = e.Parameter as ChatNavigationParameter;
            if (chatParameter == null) return;
            curentUserId = chatParameter.CurrentUserId;

            if (chatParameter.Dialog != null)
            {
                dialog = chatParameter.Dialog;
                ChatName = chatParameter.Dialog.Name;
                ChatImage = chatParameter.Dialog.Image;

                if (dialog.Messages != null && dialog.Messages.Any())
                {
                    Messages = new ObservableCollection<MessageVm>(dialog.Messages);
                }
                else
                {
                    LoadMessages(chatParameter.Dialog.Id);
                }
            }
        }

        #endregion

        #region Private methods

        private async void SendCommandExecute()
        {
            if (string.IsNullOrWhiteSpace(NewMessageText)) return;

            var msg = new MessageVm()
            {
                MessageText = NewMessageText,
                MessageType = MessageType.Outgoing,
                DateTime = DateTime.Now
            };

            Messages.Add(msg);
            dialog.Messages.Add(msg);
            dialog.LastActivity = NewMessageText;

            NewMessageText = "";
        }

        private async void LoadMessages(string dialogId)
        {
            var response = await QuickbloxClient.ChatClient.GetMessagesAsync(dialogId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Message message in response.Result.Items)
                {
                    var msg = (MessageVm)message;
                    msg.MessageType = message.SenderId == curentUserId ? MessageType.Outgoing : MessageType.Incoming;
                    Messages.Add(msg);
                    dialog.Messages.Add(msg);
                }
            }
        }

        #endregion

    }
}
