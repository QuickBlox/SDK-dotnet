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
        /// Event when a new message is received.
        /// </summary>
        public event MessageEventHandler MessageReceived;

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
        public void SendMessage(string message)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);

            xmppClient.SendMessage(new Sharp.Xmpp.Jid(otherUserJid), message, extraParams.ToString(), null, dialogId, Sharp.Xmpp.Im.MessageType.Chat);
        }

        /// <summary>
        /// Sends an attachemnt to another user.
        /// </summary>
        /// <param name="attachment">Attachment</param>
        /// <returns></returns>
        public void SendAttachemnt(AttachmentTag attachment)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.Add(attachment);

            xmppClient.SendMessage(new Sharp.Xmpp.Jid(otherUserJid), "Attachment", extraParams.ToString(), null, dialogId, Sharp.Xmpp.Im.MessageType.Chat);
        }

        #region Notify ChatState

        /// <summary>
        /// Notifies other user that you are composing a message.
        /// </summary>
        public void NotifyIsTyping()
        {
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), Sharp.Xmpp.Extensions.ChatState.Composing);
        }

        /// <summary>
        /// Notifies other user that you are paused a message.
        /// </summary>
        public void NotifyPausedTyping()
        {
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), Sharp.Xmpp.Extensions.ChatState.Paused);
        }

        /// <summary>
        /// Notifies other user that you are active a message.
        /// </summary>
        public void NotifyActiveInChat()
        {
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), Sharp.Xmpp.Extensions.ChatState.Active);
        }

        /// <summary>
        /// Notifies other user that you are inactive a message.
        /// </summary>
        public void NotifyInactiveInChat()
        {
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), Sharp.Xmpp.Extensions.ChatState.Inactive);
        }

        /// <summary>
        /// Notifies the gone in chat.
        /// </summary>
        public void NotifyGoneInChat()
        {
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), Sharp.Xmpp.Extensions.ChatState.Gone);
        }

        #endregion

        #region Friends

        /// <summary>
        /// Adds other user to your roster, subsribes for his presence, and sends FriendRequest notification message.
        /// </summary>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public void AddToFriends(string contactName = null, bool createChatMessage = false)
        {
            var rosterItem = new RosterItem(new Jid(otherUserJid), contactName ?? otherUserId.ToString());
            quickbloxClient.ChatXmppClient.AddContact(rosterItem);

            SubsribeForPresence();

            if (createChatMessage)
            {
                SendFriendsNotification("Contact request", NotificationTypes.FriendsRequest);
            }
        }

        /// <summary>
        /// Adds other user to your roster, accepts presence subscription request, and sends FriendAccepted notification message.
        /// </summary>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public void AcceptFriend(string contactName = null, bool createChatMessage = false)
        {
            var rosterItem = new RosterItem(new Jid(otherUserJid), contactName ?? otherUserId.ToString());
            quickbloxClient.ChatXmppClient.AddContact(rosterItem);

            ApproveSubscribtionRequest();
            SubsribeForPresence();

            if (createChatMessage)
            {
                SendFriendsNotification("Request accepted", NotificationTypes.FriendsAccept);
            }
        }

        /// <summary>
        /// Rejects subsription requests and sends FriendRejected notification message.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public void RejectFriend(bool createChatMessage)
        {
            RejectSubscribtionRequest();

            if (createChatMessage)
            {
                SendFriendsNotification("Request rejected", NotificationTypes.FriendsReject);
            }
        }

        /// <summary>
        /// Sends FriendRemoved notification messages, removes other user from your roster and unsubscribes from presence.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat message and add this message to the chat history.</param>
        /// <returns>Is operation successful</returns>
        public void DeleteFromFriends(bool createChatMessage)
        {
            quickbloxClient.ChatXmppClient.RemoveContact(otherUserId);

            if (createChatMessage)
            {
                SendFriendsNotification("Contact removed", NotificationTypes.FriendsRemove);
            }

            Unsubscribe();
        }

        #endregion

        #endregion

        #region Private methods
        
        private void SendFriendsNotification(string messageText, NotificationTypes notificationType)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)notificationType).ToString());

            xmppClient.SendMessage(new Sharp.Xmpp.Jid(otherUserJid), messageText, extraParams.ToString(), null, dialogId, Sharp.Xmpp.Im.MessageType.Chat);
        }


        private void MessagesClientOnOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            if (string.IsNullOrEmpty(messageEventArgs.Message.MessageText)) return;

            if (string.Equals(messageEventArgs.Jid.ToString() , otherUserJid) && messageEventArgs.Message.NotificationType != NotificationTypes.GroupCreate)
            {
                MessageReceived?.Invoke(this, messageEventArgs);
            }
        }

        #region Presence

        public void SubsribeForPresence()
        {
            xmppClient.RequestSubscription(new Sharp.Xmpp.Jid(otherUserJid));
        }

        public void ApproveSubscribtionRequest()
        {
            xmppClient.ApproveSubscriptionRequest(new Sharp.Xmpp.Jid(otherUserJid));
        }

        public void RejectSubscribtionRequest()
        {
            xmppClient.RefuseSubscriptionRequest(new Sharp.Xmpp.Jid(otherUserJid));
        }

        public void Unsubscribe()
        {
            xmppClient.RevokeSubscription(new Sharp.Xmpp.Jid(otherUserJid));
        }

        #endregion

        #endregion
    }
}
