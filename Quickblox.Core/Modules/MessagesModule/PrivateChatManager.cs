using agsXMPP;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using AgsMessage = agsXMPP.protocol.client.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;

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

        public void SendMessage(string message)
        {
            xmpp.Send(new AgsMessage(otherUserJid, agsXMPP.protocol.client.MessageType.chat, message));
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

        #endregion
    }
}
