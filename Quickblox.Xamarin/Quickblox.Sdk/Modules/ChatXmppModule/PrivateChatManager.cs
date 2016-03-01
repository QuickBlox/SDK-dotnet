using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Sharp.Xmpp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class PrivateChatManager
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;
        private readonly int otherUserId;
        private readonly string otherUserJid;
        private readonly string dialogId;

        #endregion

        /// <summary>
        /// Event when other user is typing.
        /// </summary>
        public event EventHandler OpponentStartedTyping;

        /// <summary>
        /// Event when other user has stopped typing.
        /// </summary>
        public event EventHandler OpponentPausedTyping;

        /// <summary>
        /// Event when a new message is received.
        /// </summary>
        public event EventHandler<Message> MessageReceived;

        #region Ctor

        internal PrivateChatManager(IQuickbloxClient quickbloxClient, XmppClient xmppClient, int otherUserId, string dialogId)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = ChatXmppClient.BuildJid(otherUserId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
            quickbloxClient.ChatXmppClient.MessageReceived += MessagesClientOnOnMessageReceived;
        }

        #endregion

        #region IPrivateChatManager members

        /// <summary>
        /// Sends a message to other user.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>Is operation successful</returns>
        public bool SendMessage(string message)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = message };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        /// <summary>
        /// Sends an attachemnt to another user.
        /// </summary>
        /// <param name="attachment">Attachment</param>
        /// <returns></returns>
        public bool SendAttachemnt(AttachmentTag attachment)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Attachment" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.Add(attachment);

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);

            return true;
        }

        /// <summary>
        /// Notifies other user that you are typing a message.
        /// </summary>
        public void NotifyIsTyping()
        {
            var msg = CreateNewMessage();
            var composing = new composing();
            msg.Add(composing);

            xmppClient.Send(msg);
        }

        /// <summary>
        /// Notifies other user that you've stopped typing a message.
        /// </summary>
        public void NotifyPausedTyping()
        {
            var msg = CreateNewMessage();
            var paused = new paused();
            msg.Add(paused);

            xmppClient.Send(msg);
        }

        #region Friends

        /// <summary>
        /// Adds other user to your roster, subsribes for his presence, and sends FriendRequest notification message.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <returns>Is operation successful</returns>
        public bool AddToFriends(bool createChatMessage, string contactName = null)
        {
            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            rosterManager?.AddContact(new Contact() { Name = contactName ?? otherUserId.ToString(), UserId = otherUserId });

            SubsribeForPresence();
            ApproveSubscribtionRequest();

            if (createChatMessage)
            {
                return SendFriendsNotification("Contact request", NotificationTypes.FriendsRequest);
            }

            return true;
        }

        /// <summary>
        /// Adds other user to your roster, accepts presence subscription request, and sends FriendAccepted notification message.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <returns>Is operation successful</returns>
        public bool AcceptFriend(bool createChatMessage, string contactName = null)
        {
            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            rosterManager?.AddContact(new Contact()
            {
                Name = contactName ?? otherUserId.ToString(),
                UserId = otherUserId
            });

            SubsribeForPresence();
            ApproveSubscribtionRequest();

            if (createChatMessage)
            {
                return SendFriendsNotification("Request accepted", NotificationTypes.FriendsAccept);
            }

            return true;
        }

        /// <summary>
        /// Rejects subsription requests and sends FriendRejected notification message.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public bool RejectFriend(bool createChatMessage)
        {
            RejectSubscribtionRequest();

            if (createChatMessage)
            {
                return SendFriendsNotification("Request rejected", NotificationTypes.FriendsReject);
            }

            return true;
        }

        /// <summary>
        /// Sends FriendRemoved notification messages, removes other user from your roster and unsubscribes from presence.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public bool DeleteFromFriends(bool createChatMessage)
        {
            if (createChatMessage)
            {
                return SendFriendsNotification("Contact removed", NotificationTypes.FriendsRemove);
            }

            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            rosterManager?.DeleteContact(otherUserId);

            Unsubscribe();
            SendPresenceInformation(presence.typeEnum.unsubscribed);

            return true;
        }

        #endregion

        #endregion

        #region Private methods
        
        private bool SendFriendsNotification(string messageText, NotificationTypes notificationType)
        {
            var msg = CreateNewMessage();
            var body = new body { Value = messageText };
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)notificationType).ToString());

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }


        private void MessagesClientOnOnMessageReceived(object sender, Message message1)
        {
            if (message1.IsTyping)
            {
                OpponentStartedTyping?.Invoke(this, new EventArgs());
            }

            if (message1.IsPausedTyping)
            {
                OpponentPausedTyping?.Invoke(this, new EventArgs());
            }

            if (string.IsNullOrEmpty(message1.MessageText)) return;

            if (message1.From.Contains(otherUserJid) && message1.NotificationType != NotificationTypes.GroupCreate)
            {
                MessageReceived?.Invoke(this, message1);
            }
        }

        #region Presence

        private void SubsribeForPresence()
        {
            xmppClient.RefuseSubscriptionRequest(Subsc)
            SendPresenceInformation(presence.typeEnum.subscribe);
        }

        private void ApproveSubscribtionRequest()
        {
            SendPresenceInformation(presence.typeEnum.subscribed);
        }

        private void RejectSubscribtionRequest()
        {
            SendPresenceInformation(presence.typeEnum.unsubscribed);
        }

        private void Unsubscribe()
        {
            SendPresenceInformation(presence.typeEnum.unsubscribe);
        }

        private void SendPresenceInformation(presence.typeEnum type)
        {
            xmppClient.
        }

        #endregion

        #endregion
    }
}
