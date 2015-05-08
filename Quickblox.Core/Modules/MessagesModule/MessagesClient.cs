using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Serializer;
using AgsMessage = agsXMPP.protocol.client.Message;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;
using Presence = Quickblox.Sdk.Modules.MessagesModule.Models.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient : IMessagesClient
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
            var xmpp = new XmppClientConnection(chatEndpoint);
            OpenConnection(xmpp, userId, password, applicationId);
        }

        public async Task Connect(int userId, string password, int applicationId, string chatEndpoint, TimeSpan timeout)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            var xmpp = new XmppClientConnection(chatEndpoint);
            xmpp.OnLogin += sender =>
            {
                tcs.SetResult(null);
            };
            xmpp.OnAuthError += (sender, element) => tcs.SetException(new QuickbloxSdkException(element.Value));
            xmpp.OnError += (sender, exception) => tcs.SetException(new QuickbloxSdkException("Error connecting to xmpp server.", exception));

            var timer = new Timer(state =>
            {
                if (tcs.Task.Status == TaskStatus.WaitingForActivation)
                    tcs.SetCanceled();
            },
            null, timeout, new TimeSpan(0, 0, 0, 0, -1));

            OpenConnection(xmpp, userId, password, applicationId);

            await tcs.Task;

        }

        public IPrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new QuickbloxSdkException("Xmpp connection is not ready.");

            string otherUserJid = string.Format(qbJidPattern, otherUserId, appId);

            return new PrivateChatManager(xmppConnection, otherUserJid);
        }

        public IGroupChatManager GetGroupChatManager(string groupJid)
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

        private void OpenConnection(XmppClientConnection xmpp, int userId, string password, int applicationId)
        {
            xmppConnection = xmpp;
            xmppConnection.OnLogin += XmppConnectionOnOnLogin;
            xmppConnection.OnMessage += XmppConnectionOnOnMessage;
            xmppConnection.OnPresence += XmppConnectionOnOnPresence;
            xmppConnection.OnRosterStart += XmppConnectionOnOnRosterStart;
            xmppConnection.OnRosterItem += XmppConnectionOnOnRosterItem;
            xmppConnection.OnRosterEnd += XmppConnectionOnOnRosterEnd;
            xmppConnection.OnAuthError += (sender, element) => { throw new QuickbloxSdkException("Failed to authenticate: " + element.ToString()); };
            this.appId = applicationId;

#if DEBUG
            xmppConnection.OnReadXml += XmppConnectionOnOnReadXml;
            xmppConnection.OnWriteXml += XmppConnectionOnOnWriteXml;
#endif

            xmppConnection.Open(string.Format(qbJidUserPattern, userId, applicationId), password);
        }

        private void XmppConnectionOnOnLogin(object sender)
        {
            IsConnected = true;
            var handler = OnConnected;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void XmppConnectionOnOnMessage(object sender, AgsMessage msg)
        {
            string extraParams = msg.GetTag("extraParams");
            var attachments = new List<Attachment>();
            if (!string.IsNullOrEmpty(extraParams))
            {
                XmlReaderSettings settings = new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Fragment};
                using (XmlReader reader = XmlReader.Create(new StringReader(extraParams), settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "Attachment")
                            {
                                var attachmentXml = reader.ReadOuterXml();
                                var xmlSerializer = new XmlSerializer();
                                var attachment = xmlSerializer.Deserialize<Attachment>(attachmentXml);
                                if(attachment != null) attachments.Add(attachment);
                            }
                        }
                    }
                }
            }

            

            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, new Message {From = msg.From.ToString(), To = msg.To.ToString(), MessageText = msg.Body, Attachments = attachments.ToArray()});
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
