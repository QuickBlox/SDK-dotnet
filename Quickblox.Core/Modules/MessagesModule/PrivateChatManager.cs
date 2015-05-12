using agsXMPP;
using agsXMPP.protocol.iq.privacy;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Serializer;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Action = agsXMPP.protocol.iq.privacy.Action;
using AgsMessage = agsXMPP.protocol.client.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;
using Type = agsXMPP.protocol.iq.privacy.Type;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class PrivateChatManager : IPrivateChatManager
    {
        private string banListName = "banList";

        #region Fields

        private readonly XmppClientConnection xmpp;
        private readonly string otherUserJid;

        #endregion

        #region Ctor

        public PrivateChatManager(XmppClientConnection xmppConnection, string otherUserJid)
        {
            this.otherUserJid = otherUserJid;
            xmpp = xmppConnection;
        }

        #endregion

        #region IPrivateChatManager members

        public void SendMessage(string message, Attachment attachment = null)
        {
            var msg = new AgsMessage(otherUserJid, agsXMPP.protocol.client.MessageType.chat, message);
            if (attachment != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer();
                string attachemntXml = xmlSerializer.Serialize(attachment);
                msg.AddTag("extraParams", attachemntXml);
            }

            xmpp.Send(msg);
        }

        public void SubsribeForPresence()
        {
            SendPresenceInformation(PresenceType.subscribe);
        }

        public void ApproveSubscribtionRequest()
        {
            SendPresenceInformation(PresenceType.subscribe);
        }

        public void DeclineSubscribtionRequest()
        {
            SendPresenceInformation(PresenceType.unsubscribed);
        }

        public void Unsubscribe()
        {
            SendPresenceInformation(PresenceType.unsubscribe);
        }

        public void SendPresenceInformation(PresenceType presenceType)
        {
            xmpp.Send(new AgsPresence { Type = (agsXMPP.protocol.client.PresenceType)presenceType, To = new Jid(otherUserJid) });
        }

        public async Task Block()
        {
            var list = await GetBanListAsync() ?? new List();

            list.AddItem(new Item(Action.deny, 0, Type.jid, otherUserJid));
            var privacyManager = new PrivacyManager(xmpp);
            privacyManager.AddList(banListName, list.GetItems());
            privacyManager.ChangeActiveList(banListName);
            privacyManager.ChangeDefaultList(banListName);
        }

        public async Task Unblock()
        {
            var list = await GetBanListAsync() ?? new List();

            if (list.GetItems().Any(i => i.Val == otherUserJid))
            {
                var privacyManager = new PrivacyManager(xmpp);
                privacyManager.AddList(banListName, list.GetItems().Where(i => i.Val != otherUserJid).ToArray());
                privacyManager.ChangeActiveList(banListName);
                privacyManager.ChangeDefaultList(banListName);
            }
            
        }

        #endregion

        #region Private methods

        private async Task<List> GetBanListAsync()
        {
            TimeSpan timeout = new TimeSpan(0, 0, 5);

            TaskCompletionSource<List> tcs = new TaskCompletionSource<List>();

            xmpp.OnIq += (sender, iq) =>
            {
                if (iq.Query != null &&  iq.Query.Namespace.Contains("jabber:iq:privacy"))
                {
                    Privacy privacy = iq.Query as Privacy;
                    if (privacy != null && tcs.Task.Status == TaskStatus.WaitingForActivation)
                    {
                        tcs.SetResult(privacy.GetList().FirstOrDefault(l => l.Name == banListName));
                    }

                }
            };

            PrivacyManager p = new PrivacyManager(xmpp);
            p.GetList(banListName);

            var timer = new Timer(state =>
            {
                if (tcs.Task.Status == TaskStatus.WaitingForActivation)
                    tcs.SetResult(null);
            },
                null, timeout, new TimeSpan(0, 0, 0, 0, -1));

            return await tcs.Task;
        }

        #endregion

    }
}
