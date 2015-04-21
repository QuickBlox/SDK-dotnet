using System;
using System.Diagnostics;
using agsXMPP;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        private QuickbloxClient quickbloxClient;
        private XmppClientConnection xmpp;
        private int appId;


        public event EventHandler OnInitialized;


        public bool IsConnected { get; private set; }

        public MessagesClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        public void Connect(int userId, string password, int appId, string chatEndpoint)
        {
            xmpp = new XmppClientConnection(chatEndpoint);
            xmpp.OnLogin += XmppOnOnLogin;
            xmpp.OnAuthError += (sender, element) => { };
            this.appId = appId;

            xmpp.Open(string.Format("{0}-{1}", userId, appId), password); 
        }

        public PrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            if(xmpp == null || !xmpp.Authenticated)
                throw new Exception("Xmpp connection is not ready.");

            return new PrivateChatManager(xmpp, otherUserId, appId);
        }

        public GroupChatManager GetGroupChatManager(string groupId)
        {
            throw new NotImplementedException();
        }

        private void XmppOnOnLogin(object sender)
        {
            IsConnected = true;
            var handler = OnInitialized;
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}
