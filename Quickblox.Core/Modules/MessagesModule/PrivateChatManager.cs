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

        public event MessageHandler OnMessageReceived;

        public event EventHandler OnInitialized;

        public event EventHandler OnPresense;

        public PrivateChatManager(int userId, string password, int otherUserId, int appId, string chatEndpoint)
        {
            otherUserJid = string.Format("{0}-{1}@chat.quickblox.com", otherUserId, appId);

            XmppClientConnection xmpp = new XmppClientConnection(chatEndpoint);
            xmpp.OnLogin += XmppOnOnLogin;
            xmpp.OnAuthError += (sender, element) => { };
            xmpp.OnMessage += XmppOnOnMessage;

            xmpp.Open(string.Format("{0}-{1}", userId, appId), password); 
        }

        private void XmppOnOnMessage(object sender, Message msg)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, msg);
        }

        private void XmppOnOnLogin(object sender)
        {
            isInitialized = true;
            var handler = OnInitialized;
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
