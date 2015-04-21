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
        private readonly XmppClientConnection xmpp;
        private readonly string otherUserJid;

        public event EventHandler<Message> OnMessageReceived;
        
        public event EventHandler OnPresense;

        

        public PrivateChatManager(XmppClientConnection xmppConnection, int otherUserId, int appId)
        {
            otherUserJid = string.Format("{0}-{1}@chat.quickblox.com", otherUserId, appId);

            xmpp = xmppConnection;
            xmpp.OnMessage += XmppOnOnMessage;
        }

        private void XmppOnOnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, new Message() {MessageText = msg.Body});
        }

        

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

    }
}
