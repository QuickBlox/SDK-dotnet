﻿using QMunicate.Core.Command;
using QMunicate.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.MessageService;

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
        private IPrivateChatManager privateChatManager;

        #endregion

        #region Ctor

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageVm>();
            SendCommand = new RelayCommand(SendCommandExecute, () => !IsLoading);
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

        public string ChatImage
        {
            get { return chatImage; }
            set { Set(ref chatImage, value); }
        }

        public RelayCommand SendCommand { get; set; }

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

            curentUserId = chatParameter.CurrentUserId;

            if (chatParameter.Dialog != null)
            {
                dialog = chatParameter.Dialog;
                ChatName = chatParameter.Dialog.Name;
                ChatImage = chatParameter.Dialog.Image;

                int otherUserId = dialog.OccupantIds.FirstOrDefault(id => id != curentUserId);
                if (otherUserId != 0)
                {
                    privateChatManager = QuickbloxClient.MessagesClient.GetPrivateChatManager(otherUserId);
                    privateChatManager.OnMessageReceived += ChatManagerOnOnMessageReceived;
                }

                await LoadMessages(chatParameter.Dialog.Id);
            }

            IsLoading = false;
        }

        private async Task LoadMessages(string dialogId)
        {
            var response = await QuickbloxClient.ChatClient.GetMessagesAsync(dialogId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Message message in response.Result.Items)
                {
                    var msg = (MessageVm)message;
                    msg.MessageType = message.SenderId == curentUserId ? MessageType.Outgoing : MessageType.Incoming;
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
                var messageService = Factory.CommonFactory.GetInstance<IMessageService>();
                await messageService.ShowAsync("Message", "This is a public group dialog. You cannot send messages to public groups yet.");
                return;
            }

            var msg = new MessageVm()
            {
                MessageText = NewMessageText,
                MessageType = MessageType.Outgoing,
                DateTime = DateTime.Now
            };

            Messages.Add(msg);
            dialog.LastActivity = NewMessageText;

            privateChatManager.SendMessage(NewMessageText);

            NewMessageText = "";
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