using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using System;
using System.Threading.Tasks;
using XMPP.common;
using XMPP.tags.jabber.client;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class PrivateChatManager : IPrivateChatManager
    {
        #region Fields

        private XMPP.Client xmppClient;
        private string banListName = "banList";
        private readonly string otherUserJid;

        #endregion

        public event EventHandler<Message> OnMessageReceived;

        #region Ctor

        public PrivateChatManager(XMPP.Client xmppClient, string otherUserJid)
        {
            this.otherUserJid = otherUserJid;
            this.xmppClient = xmppClient;
            this.xmppClient.OnReceive += XmppClientOnOnReceive;
        }

        #endregion

        #region IPrivateChatManager members

        public bool SendMessage(string message, Attachment attachment = null)
        {
            if (attachment != null)
            {
                throw new NotImplementedException("Attachments are not supported yet");

                //var msg = new AgsMessage(otherUserJid, agsXMPP.protocol.client.MessageType.chat, message);
                //if (attachment != null)
                //{
                //    XmlSerializer xmlSerializer = new XmlSerializer();
                //    string attachemntXml = xmlSerializer.Serialize(attachment);
                //    msg.AddTag("extraParams", attachemntXml);
                //}

                //xmpp.Send(msg);
            }

            var msg = new message
            {
                to = otherUserJid,
                type = XMPP.tags.jabber.client.message.typeEnum.chat
            };

            var body = new body {Value = message};
            
            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory {Value = "1"});
            
            msg.Add(body, extraParams);
            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        #region Presence

        public void SubsribeForPresence()
        {
            SendPresenceInformation(presence.typeEnum.subscribe);
        }

        public void ApproveSubscribtionRequest()
        {
            SendPresenceInformation(presence.typeEnum.subscribed);
        }

        public void DeclineSubscribtionRequest()
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

        private void XmppClientOnOnReceive(object sender, TagEventArgs tagEventArgs)
        {
            var message = tagEventArgs.tag as message;
            if (message != null && message.from.Contains(otherUserJid))
            {
                var handler = OnMessageReceived;
                if (handler != null)
                    handler(this, new Message { From = message.from, To = message.to, MessageText = message.body });
                return;
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
