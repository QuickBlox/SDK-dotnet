using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using XMPP;
using XMPP.common;
using XMPP.tags.jabber.client;
using AgsMessage = agsXMPP.protocol.client.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;
using Presence = Quickblox.Sdk.Modules.MessagesModule.Models.Presence;
using PresenceType = Quickblox.Sdk.Modules.MessagesModule.Models.PresenceType;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient : IMessagesClient
    {
        #region Fields

        private QuickbloxClient quickbloxClient;
        private XmppClientConnection xmppConnection;
        private XMPP.Client xmppClient;
        private int appId;
        readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");
        private const string qbJidPattern = @"{0}-{1}@chat.quickblox.com";

        #endregion

        #region Events

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

#if DEBUG
        public string DebugClientName { get; set; }
#endif

        #endregion

        #region Public methods

        public async Task Connect(string chatEndpoint, int userId, int applicationId, string password)
        {
            var timeout = new TimeSpan(0, 0, 60);
            var tcs = new TaskCompletionSource<object>();
            XMPP.Client xmppClient = new XMPP.Client();
            xmppClient.OnReady += (sender, args) => 
            {
                if (tcs.Task.Status == TaskStatus.WaitingForActivation)
                    tcs.TrySetResult(null);
            };

            var timer = new Timer(state =>
            {
                var myTcs = (TaskCompletionSource<object>)state;
                if (myTcs.Task.Status == TaskStatus.WaitingForActivation)
                    myTcs.TrySetException(new QuickbloxSdkException("Failed to fully initialize xmpp connection because of timeout."));
            },
            tcs, timeout, new TimeSpan(0, 0, 0, 0, -1));

            OpenConnection(xmppClient, chatEndpoint, userId, applicationId, password);

            await tcs.Task;
        }

        public IPrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            string otherUserJid = string.Format(qbJidPattern, otherUserId, appId);
            return new PrivateChatManager(xmppClient, otherUserJid);
        }

        public IGroupChatManager GetGroupChatManager(string groupJid)
        {
            throw new NotImplementedException();

            if (xmppConnection == null || !xmppConnection.Authenticated)
                throw new QuickbloxSdkException("Xmpp connection is not ready.");

            return new GroupChatManager(xmppConnection, groupJid);
        }

        public void ReloadContacts()
        {
            throw new NotImplementedException();

            xmppConnection.Send(new IQ(IqType.get) { Query = new Roster() });
        }

        public void AddContact(Contact contact)
        {
            throw new NotImplementedException();

            string jid = string.Format(qbJidPattern, contact.UserId, appId);
            var roster = new Roster();
            roster.AddRosterItem(new RosterItem(new Jid(jid), contact.Name));
            xmppConnection.Send(new IQ(IqType.set) { Query = roster });
        }

        public void DeleteContact(int userId)
        {
            throw new NotImplementedException();

            string jid = string.Format(qbJidPattern, userId, appId);
            var roster = new Roster();
            roster.AddRosterItem(new RosterItem(new Jid(jid)) {Subscription = SubscriptionType.remove});
            xmppConnection.Send(new IQ(IqType.set) { Query = roster });
        }

        #endregion

        #region Private methods

        private void OpenConnection(Client client, string chatEndpoint, int userId, int applicationId,
            string password)
        {
            appId = applicationId;

            client.Settings.Hostname = chatEndpoint;
            client.Settings.SSL = false;
            client.Settings.OldSSL = false;
            client.Settings.Port = 5222;
            client.Settings.AuthenticationTypes = MechanismType.Plain;
            client.Settings.Id = string.Format(qbJidPattern, userId, applicationId);
            client.Settings.Password = password;

            client.OnReceive += ClientOnOnReceive;
            client.OnReady += (sender, args) => IsConnected = true;
            client.OnError +=
                (sender, args) =>
                {
                    throw new QuickbloxSdkException(string.Format("XMPP connection exception. Message: {0}. Type: {1}",
                        args.message, args.type));
                };

#if DEBUG
            client.OnLogMessage +=
                (sender, args) => Debug.WriteLine("XMPP {0} LOG: {1} {2}", DebugClientName, args.type, args.message);
#endif

            xmppClient = client;
            IsConnected = false;
            client.Connect();
        }

        private void ClientOnOnReceive(object sender, TagEventArgs tagEventArgs)
        {
            var message = tagEventArgs.tag as message;
            if (message != null)
            {
                var handler = OnMessageReceived;
                if (handler != null)
                    handler(this, new Message { From = message.from, To = message.to, MessageText = message.body });
            }
        }

        #endregion

        #region Ags methods

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

    }

}
