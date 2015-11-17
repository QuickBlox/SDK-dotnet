﻿using System;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Interfaces;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using XMPP.tags.jabber.client;
using XMPP.tags.jabber.protocol.chatstates;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class PrivateChatManager : IPrivateChatManager
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        private readonly int otherUserId;
        private readonly string otherUserJid;
        private readonly string dialogId;

        #endregion

        public event EventHandler OnIsTyping;
        public event EventHandler OnPausedTyping;
        public event EventHandler<Message> OnMessageReceived;

        #region Ctor

        internal PrivateChatManager(IQuickbloxClient quickbloxClient, XMPP.Client xmppClient, int otherUserId, string dialogId)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = string.Format("{0}-{1}@{2}", otherUserId, quickbloxClient.ChatXmppClient.ApplicationId, quickbloxClient.ChatXmppClient.ChatEndpoint);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
            quickbloxClient.ChatXmppClient.OnMessageReceived += MessagesClientOnOnMessageReceived;
        }

        #endregion

        #region IPrivateChatManager members

        public bool SendMessage(string message)
        {
            var msg = CreateNewMessage();

            var body = new body {Value = message};
            
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory {Value = "1"});
            extraParams.Add(new DialogId {Value = dialogId});
            
            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public void NotifyIsTyping()
        {
            var msg = CreateNewMessage();
            var composing = new composing();
            msg.Add(composing);

            xmppClient.Send(msg);
        }

        public void NotifyPausedTyping()
        {
            var msg = CreateNewMessage();
            var paused = new paused();
            msg.Add(paused);

            xmppClient.Send(msg);
        }

        #region Friends

        public bool AddToFriends(string friendName = null)
        {
            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            if (rosterManager != null)
            {
                rosterManager.AddContact(new Contact() { Name = friendName ?? otherUserId.ToString(), UserId = otherUserId });
            }

            SubsribeForPresence();

            var msg = CreateNewMessage();
            var body = new body {Value = "Contact request"};
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory {Value = "1"});
            extraParams.Add(new DialogId {Value = dialogId});
            extraParams.Add(new NotificationType {Value = ((int) NotificationTypes.FriendsRequest).ToString()});

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public bool AcceptFriend(string friendName = null)
        {
            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            if (rosterManager != null)
            {
                rosterManager.AddContact(new Contact()
                {
                    Name = friendName ?? otherUserId.ToString(),
                    UserId = otherUserId
                });
            }

            ApproveSubscribtionRequest();

            var msg = CreateNewMessage();
            var body = new body {Value = "Request accepted"};
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory {Value = "1"});
            extraParams.Add(new DialogId {Value = dialogId});
            extraParams.Add(new NotificationType {Value = ((int) NotificationTypes.FriendsAccept).ToString()});

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public bool RejectFriend()
        {
            RejectSubscribtionRequest();

            var msg = CreateNewMessage();
            var body = new body {Value = "Request rejected"};
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory {Value = "1"});
            extraParams.Add(new DialogId {Value = dialogId});
            extraParams.Add(new NotificationType {Value = ((int) NotificationTypes.FriendsReject).ToString()});

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public bool DeleteFromFriends()
        {
            var msg = CreateNewMessage();
            var body = new body { Value = "Contact removed" };
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });
            extraParams.Add(new DialogId { Value = dialogId });
            extraParams.Add(new NotificationType { Value = ((int)NotificationTypes.FriendsRemove).ToString() });

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);

            var rosterManager = quickbloxClient.ChatXmppClient as IRosterManager;
            if (rosterManager != null)
            {
                rosterManager.DeleteContact(otherUserId);
            }

            Unsubscribe();
            SendPresenceInformation(presence.typeEnum.unsubscribed);
            
            return true;
        }

        public bool NotifyAboutGroupCreation(string createdDialogId)
        {
            var msg = CreateNewMessage();
            var body = new body { Value = "Notification message" };
            var extraParams = new ExtraParams();
            extraParams.Add(new DialogId { Value = createdDialogId });
            extraParams.Add(new NotificationType { Value = ((int)NotificationTypes.GroupCreate).ToString() });

            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        #endregion

        #endregion

        #region Private methods

        private message CreateNewMessage()
        {
            return new message
            {
                to = otherUserJid,
                type = message.typeEnum.chat,
                id = MongoObjectIdGenerator.GetNewObjectIdString()
            };
        }

        private void MessagesClientOnOnMessageReceived(object sender, Message message1)
        {
            if (message1.IsTyping)
            {
                var handler = OnIsTyping;
                if (handler != null) handler(this, new EventArgs());
            }

            if (message1.IsPausedTyping)
            {
                var handler = OnPausedTyping;
                if (handler != null) handler(this, new EventArgs());
            }

            if (string.IsNullOrEmpty(message1.MessageText)) return;

            if (message1.From.Contains(otherUserJid) && message1.NotificationType != NotificationTypes.GroupCreate)
            {
                var handler = OnMessageReceived;
                if (handler != null)
                    handler(this, message1);
            }
        }

        #region Presence

        private void SubsribeForPresence()
        {
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
            xmppClient.Send(new presence { type = type, to = otherUserJid });
        }

        #endregion

        #endregion
    }
}
