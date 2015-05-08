using System;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.privacy;
using agsXMPP.Xml.Dom;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Serializer;
using Action = agsXMPP.protocol.iq.privacy.Action;
using AgsMessage = agsXMPP.protocol.client.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;
using Type = agsXMPP.protocol.iq.privacy.Type;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class PrivateChatManager : IPrivateChatManager
    {
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

        public void Block()
        {
            PrivacyManager p = new PrivacyManager(xmpp);

            p.AddList("testList", new Item[] { new Item(Action.deny, 0, Type.jid, "6666-546@chat.quickblox.com") });
            p.ChangeActiveList("testList");
            
        }


        #endregion
    }
}
