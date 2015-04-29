using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class PrivateChatManager
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

        #region Public methods

        public bool SendMessage(string message)
        {
            if (!xmpp.Authenticated)
                return false;

            xmpp.Send(new agsXMPP.protocol.client.Message(otherUserJid, agsXMPP.protocol.client.MessageType.chat, message));
            return true;
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
            xmpp.Send(new agsXMPP.protocol.client.Presence() { Type = (agsXMPP.protocol.client.PresenceType)presenceType, To = new Jid(otherUserJid) });
        }

        public void TurnOnAutoPresense()
        {

        }

        public void TurnOffAutoPresense()
        {

        }

        #endregion


    }
}
