using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Core.DependencyInjection;
using QMunicate.Core.Observable;
using QMunicate.Helper;
using QMunicate.Services;
using Quickblox.Sdk;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;

namespace QMunicate.ViewModels.PartialViewModels
{
    /// <summary>
    /// MessageCollectionViewModel acts as a holder for current dialog's messages.
    /// Allows to load them from a dialog or to add manually.
    /// Proper notifications messages are generated automatically.
    /// </summary>
    public class MessageCollectionViewModel : ObservableObject
    {
        #region Fields

        private ObservableCollection<DayOfMessages> messages;

        #endregion

        #region Ctor

        public MessageCollectionViewModel()
        {
            Messages = new ObservableCollection<DayOfMessages>();
        }

        #endregion

        #region Properties

        public ObservableCollection<DayOfMessages> Messages
        {
            get { return messages; }
            set { Set(ref messages, value); }
        }


        #endregion

        #region Public methods

        public async Task LoadMessages(string dialogId)
        {
            var retrieveMessagesRequest = new RetrieveMessagesRequest();
            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new FieldFilter<string>(() => new Message().ChatDialogId, dialogId));
            aggregator.Filters.Add(new SortFilter<long>(SortOperator.Desc, () => new Message().DateSent));
            retrieveMessagesRequest.Filter = aggregator;

            var quickbloxClient = ServiceLocator.Locator.Get<IQuickbloxClient>();

            var response = await quickbloxClient.ChatClient.GetMessagesAsync(retrieveMessagesRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Messages.Clear();
                var messageList = new List<MessageViewModel>();
                for (int i = response.Result.Items.Length - 1; i >= 0; i--) // doing it in reverse order because we requested them from server in descending order
                {
                    var messageViewModel = await CreateMessageViewModelFromMessage(response.Result.Items[i]);
                    await GenerateProperNotificationMessages(messageViewModel, response.Result.Items[i]);
                    messageList.Add(messageViewModel);
                }
                InitializeMessagesFromList(messageList);
            }
        }

        public async Task AddNewMessage(MessageViewModel messageViewModel)
        {
            await AddMessage(messageViewModel);
        }

        /// <summary>
        /// Adds a new message to Messages collection and generates proper message texts for Notification messages.
        /// </summary>
        /// <param name="messageViewModel"></param>
        /// <param name="originalMessage"></param>
        /// <returns></returns>
        public async Task AddNewMessageAndCorrectText(MessageViewModel messageViewModel, Message originalMessage = null)
        {
            await AddMessage(messageViewModel, originalMessage);
        }

        #endregion

        #region Private methods

        private void InitializeMessagesFromList(IEnumerable<MessageViewModel> messageList)
        {
            IEnumerable<DayOfMessages> groups =
            from msg in messageList
            group msg by msg.DateTime.Date into messageGroup
            select new DayOfMessages(messageGroup)
            {
                Date = messageGroup.Key
            };

            Messages = new ObservableCollection<DayOfMessages>(groups);
        }

        private async Task AddMessage(MessageViewModel messageViewModel, Message originalMessage = null)
        {
            if (originalMessage != null)
            {
                await GenerateProperNotificationMessages(messageViewModel, originalMessage);
            }

            var messageGroup = Messages.FirstOrDefault(msgGroup => msgGroup.Date.Date == messageViewModel.DateTime.Date);
            if (messageGroup == null)
            {
                messageGroup = new DayOfMessages { Date = messageViewModel.DateTime.Date };
                Messages.Add(messageGroup);
            }
            messageGroup.Add(messageViewModel);
        }

        #region Notification messages generation

        private async Task GenerateProperNotificationMessages(MessageViewModel messageViewModel, Message originalMessage)
        {
            switch (originalMessage.NotificationType)
            {
                case NotificationTypes.FriendsRequest:
                    messageViewModel.MessageText = await BuildFriendsRequestMessage(originalMessage, messageViewModel.MessageType);
                    break;

                case NotificationTypes.FriendsAccept:
                    messageViewModel.MessageText = BuildFriendsAcceptMessage(messageViewModel.MessageType);
                    break;

                case NotificationTypes.FriendsReject:
                    messageViewModel.MessageText = BuildFriendsRejectMessage(messageViewModel.MessageType);
                    break;

                case NotificationTypes.GroupCreate:
                    messageViewModel.MessageText = await BuildGroupCreateMessage(originalMessage);
                    break;

                case NotificationTypes.GroupUpdate:
                    messageViewModel.MessageText = await BuildGroupUpdateMessage(originalMessage);
                    break;
            }
        }

        private async Task<string> BuildFriendsRequestMessage(Message message, MessageType messageType)
        {
            if (messageType == MessageType.Outgoing) return "Your request has been sent";

            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var senderUser = await cachingQbClient.GetUserById(GetSenderId(message));

            return string.Format("{0} has sent a request to you", senderUser == null ? null : senderUser.FullName);
        }

        private string BuildFriendsAcceptMessage(MessageType messageType)
        {
            return messageType == MessageType.Outgoing ? "You have accepted a request" : "Your request has been accepted";
        }

        private string BuildFriendsRejectMessage(MessageType messageType)
        {
            return messageType == MessageType.Outgoing ? "You have rejected a request" : "Your request has been rejected";
        }

        private async Task<string> BuildGroupCreateMessage(Message message)
        {
            int senderId = GetSenderId(message);
            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var senderUser = await cachingQbClient.GetUserById(senderId);

            var addedUsersBuilder = new StringBuilder();
            List<int> occupantsIds = ConvertStringToIntArray(message.OccupantsIds);
            foreach (var userId in occupantsIds.Where(o => o != senderId))
            {
                var user = await cachingQbClient.GetUserById(userId);
                if (user != null)
                    addedUsersBuilder.Append(user.FullName + ", ");
            }
            if (addedUsersBuilder.Length > 1)
                addedUsersBuilder.Remove(addedUsersBuilder.Length - 2, 2);

            return string.Format("{0} has added {1} to the group chat", senderUser == null ? "" : senderUser.FullName, addedUsersBuilder);
        }

        private async Task<string> BuildGroupUpdateMessage(Message message)
        {
            int senderId = GetSenderId(message);
            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var senderUser = await cachingQbClient.GetUserById(senderId);

            string messageText = null;
            if (!string.IsNullOrEmpty(message.RoomName))
                messageText = string.Format("{0} has changed the chat name to {1}", senderUser == null ? "" : senderUser.FullName, message.RoomName);

            if (!string.IsNullOrEmpty(message.RoomPhoto))
                messageText = string.Format("{0} has changed the chat picture", senderUser == null ? "" : senderUser.FullName);

            if (!string.IsNullOrEmpty(message.OccupantsIds))
            {
                var addedUsersBuilder = new StringBuilder();
                List<int> occupantsIds = ConvertStringToIntArray(message.OccupantsIds);
                foreach (var userId in occupantsIds.Where(o => o != senderId))
                {
                    var user = await cachingQbClient.GetUserById(userId);
                    if (user != null)
                        addedUsersBuilder.Append(user.FullName + ", ");
                }
                if (addedUsersBuilder.Length > 1)
                    addedUsersBuilder.Remove(addedUsersBuilder.Length - 2, 2);

                messageText = string.Format("{0} has added {1} to the group chat", senderUser == null ? "" : senderUser.FullName, addedUsersBuilder);
            }

            return messageText;
        }

        #endregion

        private async Task<MessageViewModel> CreateMessageViewModelFromMessage(Message message)
        {
            var messageViewModel = new MessageViewModel
            {
                MessageText = message.MessageText,
                DateTime = message.DateSent.ToDateTime(),
                NotificationType = message.NotificationType,
                SenderId = GetSenderId(message)
            };

            int currentUserId = SettingsManager.Instance.ReadFromSettings<int>(SettingsKeys.CurrentUserId);
            messageViewModel.MessageType = messageViewModel.SenderId == currentUserId ? MessageType.Outgoing : MessageType.Incoming;

            var cachingQbClient = ServiceLocator.Locator.Get<ICachingQuickbloxClient>();
            var senderUser = await cachingQbClient.GetUserById(GetSenderId(message));
            if (senderUser != null) messageViewModel.SenderName = senderUser.FullName;

            return messageViewModel;
        }

        private List<int> ConvertStringToIntArray(string occupantsIdsString)
        {
            var occupantsIds = new List<int>();
            if (string.IsNullOrEmpty(occupantsIdsString)) return occupantsIds;

            var idsStrings = occupantsIdsString.Split(',');
            foreach (string idsString in idsStrings)
            {
                int id;
                if (int.TryParse(idsString, out id))
                    occupantsIds.Add(id);
            }

            return occupantsIds;
        }

        private int GetSenderId(Message message)
        {
            if (message.SenderId != 0) return message.SenderId; // a message from REST API

            return Helpers.GetUserIdFromJid(message.From); // a message from XMPP
        }

        #endregion

    }
}
