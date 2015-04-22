using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;

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

            xmpp.Send(new agsXMPP.protocol.client.Message(otherUserJid, MessageType.chat, message));
            return true;
        }

        public void Close()
        {

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
