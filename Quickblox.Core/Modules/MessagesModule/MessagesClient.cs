using System;
using System.Diagnostics;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;
using Presence = agsXMPP.protocol.client.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;

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
        public event EventHandler<Models.Presence> OnPresenceReceived;

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
            xmppConnection.OnReadXml += XmppConnectionOnOnReadXml;
            xmppConnection.OnWriteXml += XmppConnectionOnOnWriteXml;
            xmppConnection.OnLogin += XmppConnectionOnOnLogin;
            xmppConnection.OnMessage += XmppConnectionOnOnMessage;
            xmppConnection.OnPresence += XmppConnectionOnOnPresence;
            xmppConnection.OnRosterStart += XmppConnectionOnOnRosterStart;
            xmppConnection.OnRosterItem += XmppConnectionOnOnRosterItem;
            xmppConnection.OnRosterEnd += XmppConnectionOnOnRosterEnd;
            xmppConnection.OnAuthError += (sender, element) => { };
            this.appId = appId;

            xmppConnection.Open(string.Format("{0}-{1}", userId, appId), password);
        }

        private void XmppConnectionOnOnWriteXml(object sender, string xml)
        {
            Debug.WriteLine("XMPP sent: " + xml);
        }

        private void XmppConnectionOnOnReadXml(object sender, string xml)
        {
            Debug.WriteLine("XMPP received: " + xml);
        }

        private void XmppConnectionOnOnRosterEnd(object sender)
        {
            
        }

        private void XmppConnectionOnOnRosterItem(object sender, RosterItem item)
        {

        }

        private void XmppConnectionOnOnRosterStart(object sender)
        {
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

        public void RequestContactList()
        {
            xmppConnection.Send(new agsXMPP.protocol.client.IQ(IqType.get) { Query = new Roster() });
        }

        public void AddContact(string id, string name)
        {
            string jid = string.Format("{0}-{1}@chat.quickblox.com", id, appId);
            var roster = new Roster();
            roster.AddRosterItem(new RosterItem(new Jid(jid), name));
            xmppConnection.Send(new agsXMPP.protocol.client.IQ(IqType.set) { Query = roster });
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

        private void XmppConnectionOnOnPresence(object sender, Presence pres)
        {
            var handler = OnPresenceReceived;
            if (handler != null)
                handler(this, new Models.Presence() {From = pres.From.Bare, To = pres.To.Bare, PresenceType = (PresenceType)pres.Type});
        }

        #endregion

    }

}
