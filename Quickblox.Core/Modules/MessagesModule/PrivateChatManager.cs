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
        private XmppClientConnection xmpp;
        private string otherUserJid;
        private bool isInitialized;

        public event EventHandler OnMessageReceived;

        public event EventHandler OnLogin;

        public event EventHandler OnPresense;

        public PrivateChatManager(string userId, string password, string otherUserId, string appId, string chatEndpoint)
        {
            otherUserJid = string.Format("{0}-{1}@chat.quickblox.com", otherUserId, appId);

            XmppClientConnection xmpp = new XmppClientConnection(chatEndpoint);
            xmpp.OnLogin += XmppOnOnLogin;
            xmpp.OnAuthError += (sender, element) => { };

            xmpp.Open(string.Format("{0}-{1}", userId, appId), password); 
        }

        private void XmppOnOnLogin(object sender)
        {
            isInitialized = true;
            var handler = OnLogin;
            if (handler != null)
                handler(this, new EventArgs());
        }

        public bool SendMessage(string message)
        {
            if (!isInitialized)
                return false;

            xmpp.Send(new Message(otherUserJid, MessageType.chat, message));
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

    }
}
