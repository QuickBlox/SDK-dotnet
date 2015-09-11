using System.Collections.Generic;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Sharp.Xmpp.Client;
using System.Threading.Tasks;
using System.Net;
using Quickblox.Sdk.Modules.ChatModule.Models;
using System;

namespace Quickblox.Sdk
{
    /// <summary>
    /// Private chat manager. Creates manager for personal chat
    /// </summary>
    internal class PrivateChatManager : IPrivateChatManager
    {
        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;

        private readonly Jid otherUserJid;

        private readonly int otherUserId;
        private string dialogId;

        //public event EventHandler<Message> OnMessageReceived;

        internal PrivateChatManager(IQuickbloxClient quickbloxClient, XmppClient xmppClient, int otherUserId, string dialogId)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = quickbloxClient.MessagesClient.BuildJid(otherUserId);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
        }

        public void SendMessage(string body, string subject = null, string thread = null, MessageType messageType = MessageType.Normal, NotificationType notificationType = NotificationType.None)
        {
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var jid = new Sharp.Xmpp.Jid(otherUserJid.ToString());
            var extraParams = new Dictionary<string, string>()
            {
                { "save_to_history", "1" },
                {"dialog_id", thread }
            };

            if (messageType == MessageType.Headline)
            {
                extraParams.Add("notification_type", ((int)notificationType).ToString());
            }

            xmppClient.SendMessage(jid, body, extraParams, subject, thread, wrappedMessageType);
        }

        public async Task<bool> AddToFriends(RosterItem item)
        {
            if (string.IsNullOrEmpty(dialogId))
            {
                var response = await quickbloxClient.ChatClient.CreateDialogAsync(item.Name, DialogType.Private, otherUserId.ToString());
                if (response.StatusCode != HttpStatusCode.Created)
                    return false;

                dialogId = response.Result.Id;
            }

           quickbloxClient.MessagesClient.AddContact(item);
           SendMessage("Contact request", thread: dialogId, messageType: MessageType.Normal, notificationType: NotificationType.FriendsRequest);
            
           return true;
        }

        public bool AcceptFriend(RosterItem item)
        {
            //var userResponse = await quickbloxClient.UsersClient.GetUserByIdAsync(otherUserId);
            //if (userResponse.StatusCode != HttpStatusCode.OK) return false;

            quickbloxClient.MessagesClient.AddContact(item);

            SendMessage("Request accepted", thread: dialogId, messageType: MessageType.Normal, notificationType: NotificationType.FriendsAccept);
            return true;
        }

        public bool RejectFriend(RosterItem item)
        {
            quickbloxClient.MessagesClient.RemoveContact(item);
            SendMessage("Request rejected", thread: dialogId, messageType: MessageType.Normal, notificationType: NotificationType.FriendsReject);
            
            return true;
        }

        public async Task<bool> RemoveFriend(RosterItem item)
        {
            if (string.IsNullOrEmpty(dialogId))
            {
                var response = await quickbloxClient.ChatClient.DeleteDialogAsync(dialogId);
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
            }

            quickbloxClient.MessagesClient.RemoveContact(item);
            return true;
        }
    }
}


