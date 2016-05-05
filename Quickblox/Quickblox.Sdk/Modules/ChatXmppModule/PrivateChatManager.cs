using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Xmpp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xmpp.Extensions;
using Xmpp.Im;
using Xmpp;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class PrivateChatManager
    {
        #region Fields

        private QuickbloxClient quickbloxClient;
        private readonly int otherUserId;
        private readonly string otherUserJid;
        private readonly string dialogId;

        #endregion

        /// <summary>
        /// Event when a new XmppMessage is received.
        /// </summary>
        public event MessageEventHandler MessageReceived;

        #region Ctor

        internal PrivateChatManager(QuickbloxClient quickbloxClient, int otherUserId, string dialogId)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = ChatXmppClient.BuildJid(otherUserId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            quickbloxClient.ChatXmppClient.MessageReceived += MessagesClientOnOnMessageReceived;
        }

        #endregion

        /// <summary>
        /// Sends a XmppMessage to other user.
        /// </summary>
        /// <param name="message">XmppMessage text</param>
        /// <returns>Is operation successful</returns>
        public void SendMessage(string message)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);

            quickbloxClient.ChatXmppClient.SendMessage(otherUserJid, message, extraParams.ToString(), dialogId, null);
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

            quickbloxClient.ChatXmppClient.SendMessage(otherUserJid, "Attachment", extraParams.ToString(), dialogId, null);
        }

        #region Notify ChatState

        /// <summary>
        /// Notifies other user that you are composing a XmppMessage.
        /// </summary>
        public void NotifyIsTyping()
        {
            quickbloxClient.ChatXmppClient.SetChatState(otherUserJid, ChatState.Composing);
        }

        /// <summary>
        /// Notifies other user that you are paused a XmppMessage.
        /// </summary>
        public void NotifyPausedTyping()
        {
            quickbloxClient.ChatXmppClient.SetChatState(otherUserJid, ChatState.Paused);
        }

        /// <summary>
        /// Notifies other user that you are active a XmppMessage.
        /// </summary>
        public void NotifyActiveInChat()
        {
            quickbloxClient.ChatXmppClient.SetChatState(otherUserJid, ChatState.Active);
        }

        /// <summary>
        /// Notifies other user that you are inactive a XmppMessage.
        /// </summary>
        public void NotifyInactiveInChat()
        {
            quickbloxClient.ChatXmppClient.SetChatState(otherUserJid, ChatState.Inactive);
        }

        /// <summary>
        /// Notifies the gone in chat.
        /// </summary>
        public void NotifyGoneInChat()
        {
            quickbloxClient.ChatXmppClient.SetChatState(otherUserJid, ChatState.Gone);
        }

        #endregion

        #region Friends

        /// <summary>
        /// Adds other user to your roster, subsribes for his presence, and sends FriendRequest notification XmppMessage.
        /// </summary>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <param name="createChatMessage">Notify an opponent with a chat XmppMessage and add this XmppMessage to the chat history.</param>
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
        /// Adds other user to your roster, accepts presence subscription request, and sends FriendAccepted notification XmppMessage.
        /// </summary>
        /// <param name="contactName">Opponents name in your contact list</param>
        /// <param name="createChatMessage">Notify an opponent with a chat XmppMessage and add this XmppMessage to the chat history.</param>
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
        /// Rejects subsription requests and sends FriendRejected notification XmppMessage.
        /// </summary>
        /// <param name="createChatMessage">Notify an opponent with a chat XmppMessage and add this XmppMessage to the chat history.</param>
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
        /// <param name="createChatMessage">Notify an opponent with a chat XmppMessage and add this XmppMessage to the chat history.</param>
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

        
        private void SendFriendsNotification(string messageText, NotificationTypes notificationType)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)notificationType).ToString());

            quickbloxClient.ChatXmppClient.SendMessage(otherUserJid, messageText, extraParams.ToString(), dialogId, null);
        }


        private void MessagesClientOnOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            if (string.IsNullOrEmpty(messageEventArgs.Message.MessageText)) return;

            if (messageEventArgs.Jid.ToString().Contains(otherUserJid) && messageEventArgs.Message.NotificationType != NotificationTypes.GroupCreate)
            {
                MessageReceived?.Invoke(this, messageEventArgs);
            }
        }

        #region Presence

        public void SubsribeForPresence()
        {
            quickbloxClient.ChatXmppClient.SetSubscribtionStatus(otherUserJid, SubscriptionMessageType.RequestSubscription);
        }

        public void ApproveSubscribtionRequest()
        {
            quickbloxClient.ChatXmppClient.SetSubscribtionStatus(otherUserJid, SubscriptionMessageType.ApproveSubscription);
        }

        public void RejectSubscribtionRequest()
        {
            quickbloxClient.ChatXmppClient.SetSubscribtionStatus(otherUserJid, SubscriptionMessageType.RefuseSubscription);
        }

        public void Unsubscribe()
        {
            quickbloxClient.ChatXmppClient.SetSubscribtionStatus(otherUserJid, SubscriptionMessageType.RevokeSubscription);
        }

        #endregion
    }
}
