using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using XMPP.tags.jabber.client;
using XMPP.tags.jabber.protocol.chatstates;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class PrivateChatManager : IPrivateChatManager
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        private string banListName = "banList";
        private readonly int otherUserId;
        private readonly string otherUserJid;
        private string dialogId;

        #endregion

        public event EventHandler OnIsTyping;
        public event EventHandler OnPausedTyping;
        public event EventHandler<Message> OnMessageReceived;

        #region Ctor

        internal PrivateChatManager(IQuickbloxClient quickbloxClient, XMPP.Client xmppClient, int otherUserId, string dialogId = null)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = string.Format("{0}-{1}@{2}", otherUserId, quickbloxClient.MessagesClient.ApplicationId, quickbloxClient.MessagesClient.ChatEndpoint);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
            quickbloxClient.MessagesClient.OnMessageReceived += MessagesClientOnOnMessageReceived;
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

        public async Task<bool> AddToFriends(string friendName)
        {
            if (string.IsNullOrEmpty(dialogId))
            {
                var response =
                    await
                        quickbloxClient.ChatClient.CreateDialogAsync(friendName, DialogType.Private,
                            otherUserId.ToString());
                if (response.StatusCode != HttpStatusCode.Created) return false;

                dialogId = response.Result.Id;
            }

            quickbloxClient.MessagesClient.AddContact(new Contact() {Name = friendName, UserId = otherUserId});
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

        public async Task<bool> AcceptFriend()
        {
            var userResponse = await quickbloxClient.UsersClient.GetUserByIdAsync(otherUserId);
            if (userResponse.StatusCode != HttpStatusCode.OK) return false;

            quickbloxClient.MessagesClient.AddContact(new Contact()
            {
                Name = userResponse.Result.User.FullName,
                UserId = otherUserId
            });
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

        public async Task<bool> RejectFriend()
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

            quickbloxClient.MessagesClient.DeleteContact(otherUserId);
            Unsubscribe();
            SendPresenceInformation(presence.typeEnum.unsubscribed);

            
            return true;
        }

        /// <summary>
        /// Notify a user about a created group dialog.
        /// </summary>
        /// <param name="createdDialogId"></param>
        /// <returns></returns>
        public async Task<bool> NotifyAboutGroupCreation(string createdDialogId)
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


        #region Presence

        public void SubsribeForPresence()
        {
            SendPresenceInformation(presence.typeEnum.subscribe);
        }

        public void ApproveSubscribtionRequest()
        {
            SendPresenceInformation(presence.typeEnum.subscribed);
        }

        public void RejectSubscribtionRequest()
        {
            SendPresenceInformation(presence.typeEnum.unsubscribed);
        }

        public void Unsubscribe()
        {
            SendPresenceInformation(presence.typeEnum.unsubscribe);
        }

        #endregion

        /// <summary>
        /// Prohibits other user from sending any messages to current user.
        /// </summary>
        /// <returns></returns>
        public async Task Block()
        {
            throw new NotImplementedException("User ban is not implemented with Ubiety.");

            //var list = await GetBanListAsync() ?? new List();

            //list.AddItem(new Item(Action.deny, 0, Type.jid, otherUserJid));
            //var privacyManager = new PrivacyManager(xmpp);
            //privacyManager.AddList(banListName, list.GetItems());
            //privacyManager.ChangeActiveList(banListName);
            //privacyManager.ChangeDefaultList(banListName);
        }

        /// <summary>
        /// Allow other user to send send messages to current user.
        /// </summary>
        /// <returns></returns>
        public async Task Unblock()
        {
            throw new NotImplementedException("User ban is not implemented with Ubiety.");

            //var list = await GetBanListAsync() ?? new List();

            //if (list.GetItems().Any(i => i.Val == otherUserJid))
            //{
            //    var privacyManager = new PrivacyManager(xmpp);
            //    privacyManager.AddList(banListName, list.GetItems().Where(i => i.Val != otherUserJid).ToArray());
            //    privacyManager.ChangeActiveList(banListName);
            //    privacyManager.ChangeDefaultList(banListName);
            //}
            
        }

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

        private void SendPresenceInformation(presence.typeEnum type)
        {
            xmppClient.Send(new presence { type = type, to = otherUserJid });
        }

        //private async Task<List> GetBanListAsync()
        //{
        //    TimeSpan timeout = new TimeSpan(0, 0, 5);

        //    TaskCompletionSource<List> tcs = new TaskCompletionSource<List>();

        //    xmpp.OnIq += (sender, iq) =>
        //    {
        //        if (iq.Query != null &&  iq.Query.Namespace.Contains("jabber:iq:privacy"))
        //        {
        //            Privacy privacy = iq.Query as Privacy;
        //            if (privacy != null && tcs.Task.Status == TaskStatus.WaitingForActivation)
        //            {
        //                tcs.SetResult(privacy.GetList().FirstOrDefault(l => l.Name == banListName));
        //            }

        //        }
        //    };

        //    PrivacyManager p = new PrivacyManager(xmpp);
        //    p.GetList(banListName);

        //    var timer = new Timer(state =>
        //    {
        //        if (tcs.Task.Status == TaskStatus.WaitingForActivation)
        //            tcs.SetResult(null);
        //    },
        //        null, timeout, new TimeSpan(0, 0, 0, 0, -1));

        //    return await tcs.Task;
        //}

        #endregion

    }
}
