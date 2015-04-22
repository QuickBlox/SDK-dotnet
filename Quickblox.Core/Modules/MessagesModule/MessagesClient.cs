using System;
using System.Diagnostics;
using agsXMPP;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        #region Fields

        private QuickbloxClient quickbloxClient;
        private XmppClientConnection xmppConnection;
        private int appId;

        #endregion

        #region Properties

        public event EventHandler OnConnected;
        public event EventHandler<Message> OnMessageReceived;

        public bool IsConnected { get; private set; }

        #endregion

        #region Ctor

        public MessagesClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods

        public void Connect(int userId, string password, int appId, string chatEndpoint)
        {
            xmppConnection = new XmppClientConnection(chatEndpoint);
            xmppConnection.OnLogin += XmppConnectionOnOnLogin;
            xmppConnection.OnMessage += XmppConnectionOnOnMessage;
            xmppConnection.OnAuthError += (sender, element) => { };
            this.appId = appId;

            xmppConnection.Open(string.Format("{0}-{1}", userId, appId), password);
        }

        public PrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new Exception("Xmpp connection is not ready.");

            string otherUserJid = string.Format("{0}-{1}@chat.quickblox.com", otherUserId, appId);

            return new PrivateChatManager(xmppConnection, otherUserJid);
        }

        public GroupChatManager GetGroupChatManager(string groupJid)
        {
            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new Exception("Xmpp connection is not ready.");

            return new GroupChatManager(xmppConnection, groupJid);
        }

        #endregion

        #region Private methods

        private void XmppConnectionOnOnLogin(object sender)
        {
            IsConnected = true;
            var handler = OnConnected;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void XmppConnectionOnOnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, new Message() {From = msg.From.Bare, To = msg.To.Bare, MessageText = msg.Body});
        }

        #endregion

    }

}
