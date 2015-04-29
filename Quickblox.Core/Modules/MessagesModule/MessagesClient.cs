using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using AgsMessage = agsXMPP.protocol.client.Message;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;
using Presence = Quickblox.Sdk.Modules.MessagesModule.Models.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        #region Fields

        private QuickbloxClient quickbloxClient;
        private XmppClientConnection xmppConnection;
        private int appId;
        readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+"); // Quickblox JID pattern
        private const string qbJidPattern = @"{0}-{1}@chat.quickblox.com";
        private const string qbJidUserPattern = "{0}-{1}";

        #endregion

        #region Events

        public event EventHandler OnConnected;
        public event EventHandler<Message> OnMessageReceived;
        public event EventHandler<Presence> OnPresenceReceived;
        public event EventHandler OnContactsLoaded;

        #endregion

        #region Ctor

        public MessagesClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Properties

        public List<Contact> Contacts { get; private set; }

        public bool IsConnected { get; private set; }

        #endregion

        #region Public methods

        public void Connect(int userId, string password, int applicationId, string chatEndpoint)
        {
            xmppConnection = new XmppClientConnection(chatEndpoint);
            
            xmppConnection.OnLogin += XmppConnectionOnOnLogin;
            xmppConnection.OnMessage += XmppConnectionOnOnMessage;
            xmppConnection.OnPresence += XmppConnectionOnOnPresence;
            xmppConnection.OnRosterStart += XmppConnectionOnOnRosterStart;
            xmppConnection.OnRosterItem += XmppConnectionOnOnRosterItem;
            xmppConnection.OnRosterEnd += XmppConnectionOnOnRosterEnd;
            xmppConnection.OnAuthError += (sender, element) => { throw new QuickbloxSdkException("Failed to authenticate: " + element.ToString());};
            this.appId = applicationId;

#if DEBUG
            xmppConnection.OnReadXml += XmppConnectionOnOnReadXml;
            xmppConnection.OnWriteXml += XmppConnectionOnOnWriteXml;
#endif

            xmppConnection.Open(string.Format(qbJidUserPattern, userId, applicationId), password);
        }

        public PrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new QuickbloxSdkException("Xmpp connection is not ready.");

            string otherUserJid = string.Format(qbJidPattern, otherUserId, appId);

            return new PrivateChatManager(xmppConnection, otherUserJid);
        }

        public GroupChatManager GetGroupChatManager(string groupJid)
        {
            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new QuickbloxSdkException("Xmpp connection is not ready.");

            return new GroupChatManager(xmppConnection, groupJid);
        }

        public void ReloadContacts()
        {
            xmppConnection.Send(new IQ(IqType.get) { Query = new Roster() });
        }

        public void AddContact(Contact contact)
        {
            string jid = string.Format(qbJidPattern, contact.UserId, appId);
            var roster = new Roster();
            roster.AddRosterItem(new RosterItem(new Jid(jid), contact.Name));
            xmppConnection.Send(new IQ(IqType.set) { Query = roster });
        }

        public void DeleteContact(int userId)
        {
            string jid = string.Format(qbJidPattern, userId, appId);
            var roster = new Roster();
            roster.AddRosterItem(new RosterItem(new Jid(jid)) {Subscription = SubscriptionType.remove});
            xmppConnection.Send(new IQ(IqType.set) { Query = roster });
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

        private void XmppConnectionOnOnMessage(object sender, AgsMessage msg)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, new Message {From = msg.From.ToString(), To = msg.To.ToString(), MessageText = msg.Body});
        }

        private void XmppConnectionOnOnPresence(object sender, AgsPresence pres)
        {
            var handler = OnPresenceReceived;
            if (handler != null)
                handler(this, new Presence {From = pres.From.ToString(), To = pres.To.ToString(), PresenceType = (PresenceType)pres.Type});
        }

        private void XmppConnectionOnOnRosterStart(object sender)
        {
            Contacts = new List<Contact>();
        }

        private void XmppConnectionOnOnRosterItem(object sender, RosterItem item)
        {
            if (item == null) return;

            var match = qbJidRegex.Match(item.Jid.Bare);
            string userId = (match.Success && match.Groups.Count > 0) ? match.Groups[1].Value : item.Jid.ToString();

            Contacts.Add(new Contact { UserId = userId, Name = item.Name });
        }

        private void XmppConnectionOnOnRosterEnd(object sender)
        {
            var handler = OnContactsLoaded;
            if (handler != null)
                handler(this, new EventArgs());
        }

        #endregion

#if DEBUG

        private void XmppConnectionOnOnWriteXml(object sender, string xml)
        {
            Debug.WriteLine("XMPP sent: " + xml);
        }

        private void XmppConnectionOnOnReadXml(object sender, string xml)
        {
            Debug.WriteLine("XMPP received: " + xml);
        }

#endif

    }

}
