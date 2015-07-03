using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using XMPP;
using XMPP.common;
using XMPP.tags.jabber.client;
using XMPP.tags.jabber.iq.roster;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    public class MessagesClient : IMessagesClient
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        private string chatEndpoint;
        private int appId;
        readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");
        private bool isReady;

        #endregion

        #region Events

        public event EventHandler<Message> OnMessageReceived;
        public event EventHandler<Presence> OnPresenceReceived;
        public event EventHandler OnContactsChanged;

        #endregion

        #region Ctor

        public MessagesClient(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
            Contacts = new List<Contact>();
        }

        #endregion

        #region Properties

        public List<Contact> Contacts { get; private set; }

        public List<Presence> Presences { get; private set; }

        public bool IsConnected { get { return xmppClient != null && xmppClient.Connected && isReady; } }

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

        public void Disconnect()
        {
            if (!IsConnected) return;

            xmppClient.Send(new presence { type = presence.typeEnum.unavailable });
            xmppClient.Disconnect();
            isReady = false;
        }

        public IPrivateChatManager GetPrivateChatManager(int otherUserId)
        {
            string otherUserJid = BuildJid(otherUserId);
            return new PrivateChatManager(xmppClient, otherUserJid);
        }

        public IGroupChatManager GetGroupChatManager(string groupJid)
        {
            return new GroupChatManager(xmppClient, groupJid);
        }

        public void ReloadContacts()
        {
            iq iq = new iq {type = iq.typeEnum.get};
            iq.Add(new query());
            xmppClient.Send(iq);
        }

        public void AddContact(Contact contact)
        {
            string jid = BuildJid(contact.UserId);

            var rosterItem = new item {jid = jid, name = contact.Name};
            var rosterQuery = new query();
            rosterQuery.Add(rosterItem);
            iq iq = new iq { type = iq.typeEnum.set };
            iq.Add(rosterQuery);

            xmppClient.Send(iq);
        }

        public void DeleteContact(int userId)
        {
            string jid = BuildJid(userId);

            var rosterItem = new item { jid = jid, subscription = item.subscriptionEnum.remove};
            var rosterQuery = new query();
            rosterQuery.Add(rosterItem);
            iq iq = new iq { type = iq.typeEnum.set };
            iq.Add(rosterQuery);

            xmppClient.Send(iq);
        }

        #endregion

        #region Private methods

        private void OpenConnection(Client client, string chatEndpointUrl, int userId, int applicationId,
            string password)
        {
            chatEndpoint = chatEndpointUrl;
            appId = applicationId;

            client.Settings.Hostname = chatEndpointUrl;
            client.Settings.SSL = false;
            client.Settings.OldSSL = false;
            client.Settings.Port = 5222;
            client.Settings.AuthenticationTypes = MechanismType.Plain;
            client.Settings.Id = BuildJid(userId);
            client.Settings.Password = password;
            
            client.OnReceive += ClientOnOnReceive;
            client.OnReady += (sender, args) => isReady = true;
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
            isReady = false;
            client.Connect();
        }

        private void ClientOnOnReceive(object sender, TagEventArgs tagEventArgs)
        {
            var message = tagEventArgs.tag as message;
            if (message != null)
            {
                OnMessage(message);
                return;
            }

            var presence = tagEventArgs.tag as presence;
            if (presence != null)
            {
                OnPresence(presence);
                return;
            }

            var iq = tagEventArgs.tag as iq;
            if (iq != null)
            {
                OnIq(iq);
                return;
            }
        }

        private void OnMessage(message message)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, new Message { From = message.from, To = message.to, MessageText = message.body });
        }

        private void OnPresence(presence presence)
        {
            var receivedPresence = new Presence
            {
                From = presence.from,
                To = presence.to,
                PresenceType = (PresenceType) presence.type
            };

            if (Presences == null) Presences = new List<Presence>();

            Presences.RemoveAll(p => p.From == receivedPresence.From);
            Presences.Add(receivedPresence);

            var handler = OnPresenceReceived;
            if (handler != null)
                handler(this, receivedPresence);
        }

        private void OnIq(iq iq)
        {
            if (iq.type == iq.typeEnum.result || iq.type == iq.typeEnum.set)
            {
                var query = iq.Element<query>(XMPP.tags.jabber.iq.roster.Namespace.query);
                if (query != null)
                {
                    if (iq.type == iq.typeEnum.result || Contacts == null)
                    {
                        Contacts = new List<Contact>();
                    }

                    foreach (var item in query.itemElements)
                    {
                        var match = qbJidRegex.Match(item.jid);

                        if (!match.Success || match.Groups.Count == 0) continue;
                        int userId;
                        if (!int.TryParse(match.Groups[1].Value, out userId)) continue;

                        Contacts.RemoveAll(c => c.UserId == userId);

                        if (item.subscription != XMPP.tags.jabber.iq.roster.item.subscriptionEnum.remove)
                        {
                            Contact contact = new Contact { Name = item.name, UserId = userId };
                            Contacts.Add(contact);
                        }
                    }

                    var handler = OnContactsChanged;
                    if (handler != null)
                        handler(this, new EventArgs());
                }
            }
        }

        private string BuildJid(int userId)
        {
            return string.Format("{0}-{1}@{2}", userId, appId, chatEndpoint);
        }

        //TODO: parse attachemnts from extraparams with Ubiety
        //private void XmppConnectionOnOnMessage(object sender, AgsMessage msg)
        //{
        //    string extraParams = msg.GetTag("extraParams");
        //    var attachments = new List<Attachment>();
        //    if (!string.IsNullOrEmpty(extraParams))
        //    {
        //        XmlReaderSettings settings = new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Fragment};
        //        using (XmlReader reader = XmlReader.Create(new StringReader(extraParams), settings))
        //        {
        //            while (reader.Read())
        //            {
        //                if (reader.NodeType == XmlNodeType.Element)
        //                {
        //                    if (reader.Name == "Attachment")
        //                    {
        //                        var attachmentXml = reader.ReadOuterXml();
        //                        var xmlSerializer = new XmlSerializer();
        //                        var attachment = xmlSerializer.Deserialize<Attachment>(attachmentXml);
        //                        if(attachment != null) attachments.Add(attachment);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    var handler = OnMessageReceived;
        //    if (handler != null)
        //        handler(this, new Message {From = msg.From.ToString(), To = msg.To.ToString(), MessageText = msg.Body, Attachments = attachments.ToArray()});
        //}

        #endregion

    }

}
