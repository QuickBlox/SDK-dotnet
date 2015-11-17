using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Modules.ChatXmppModule.Interfaces;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using XMPP;
using XMPP.common;
using XMPP.tags.jabber.client;
using XMPP.tags.jabber.iq.roster;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    /// <summary>
    /// Messages module allows users to chat with each other in private or group dialogs via XMPP protocol.
    /// </summary>
    public class ChatXmppClient : IChatXmppClient, IRosterManager
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");
        private bool isReady;
        private bool isUserDisconnected;

        #endregion

        #region Events

        /// <summary>
        /// Event occuring when a new message is received.
        /// </summary>
        public event EventHandler<Message> OnMessageReceived;

        /// <summary>
        /// Event occuring  when a presence is received.
        /// </summary>
        public event EventHandler<Presence> OnPresenceReceived;

        /// <summary>
        /// Event occuring  when your contacts in roster have changed.
        /// </summary>
        public event EventHandler OnContactsChanged;

        /// <summary>
        /// Event occuring when xmpp connection is lost.
        /// </summary>
        public event EventHandler OnDisconnected;

        #endregion

        #region Ctor

        internal ChatXmppClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
            Contacts = new List<Contact>();
            Presences = new List<Presence>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contacts list in roster.
        /// </summary>
        public List<Contact> Contacts { get; private set; }

        /// <summary>
        /// Presences list.
        /// </summary>
        public List<Presence> Presences { get; private set; }

        /// <summary>
        /// Is XMPP connection open
        /// </summary>
        public bool IsConnected { get { return xmppClient != null && xmppClient.Connected && isReady; } }

        /// <summary>
        /// XMPP chat endpoint.
        /// </summary>
        public string ChatEndpoint { get; private set; }

        /// <summary>
        /// Quickblox Application ID.
        /// </summary>
        public int ApplicationId { get; private set; }


#if DEBUG || TEST_RELEASE
        public string DebugClientName { get; set; }
#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Connects to XMPP server.
        /// </summary>
        /// <param name="chatEndpoint">XMPP chatendpoint</param>
        /// <param name="userId">User ID</param>
        /// <param name="applicationId">Quickblox application ID</param>
        /// <param name="password">User password</param>
        /// <returns>Async operation result</returns>
        public async Task Connect(int userId, string password)
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

            OpenConnection(xmppClient, quickbloxClient.ChatEndpoint, userId, quickbloxClient.ApplicationId, password);

            await tcs.Task;
        }

        /// <summary>
        /// Disconnects from XMPP server.
        /// </summary>
        public void Disconnect()
        {
            isUserDisconnected = true;
            Contacts.Clear();
            xmppClient.Send(new presence { type = presence.typeEnum.unavailable });
            xmppClient.Disconnect();
            isReady = false;
        }

        /// <summary>
        /// Creates a private one-to-one chat manager.
        /// </summary>
        /// <param name="otherUserId">Another user ID</param>
        /// <param name="dialogId">Dialog ID with another user</param>
        /// <returns>PrivateChatManager instance.</returns>
        public IPrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId)
        {
            return new PrivateChatManager(quickbloxClient, xmppClient, otherUserId, dialogId);
        }

        /// <summary>
        /// Creates a group chat manager.
        /// </summary>
        /// <param name="groupJid">Group XMPP room JID.</param>
        /// <param name="dialogId">Group dialog ID.</param>
        /// <returns>GroupChatManager</returns>
        public IGroupChatManager GetGroupChatManager(string groupJid, string dialogId)
        {
            return new GroupChatManager(quickbloxClient, xmppClient, groupJid, dialogId);
        }

        /// <summary>
        /// Requests roster contact list from server.
        /// </summary>
        public void ReloadContacts()
        {
            iq iq = new iq {type = iq.typeEnum.get};
            iq.Add(new query());
            xmppClient.Send(iq);
        }

        /// <summary>
        /// Adds a new contact to roster.
        /// </summary>
        /// <param name="contact"></param>
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

        /// <summary>
        /// Deletes a contact from roster.
        /// </summary>
        /// <param name="userId"></param>
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
            Contacts = new List<Contact>();
            Presences = new List<Presence>();

            ChatEndpoint = chatEndpointUrl;
            ApplicationId = applicationId;
            isUserDisconnected = false;

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

            client.OnDisconnected += (sender, args) =>
            {
                if(!isUserDisconnected)
                    client.Connect();

                var handler = OnDisconnected;
                if (handler != null)
                    handler(this, new EventArgs());
            };


            string debugClientName = "";
#if DEBUG || TEST_RELEASE
           debugClientName = DebugClientName;
#endif
            client.OnLogMessage += async (sender, args) => await LoggerHolder.Log(LogLevel.Debug, string.Format("XMPP {0} LOG: {1} {2}", debugClientName, args.type, args.message));

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

        private void OnMessage(message msg)
        {
            var receivedMessage = new Message();

            FillFields(msg, receivedMessage);
            FillExtraParamsFields(msg, receivedMessage);

            var handler = OnMessageReceived;
            if (handler != null)
                handler(this, receivedMessage);
        }

        private void FillFields(message source, Message result)
        {
            result.From = source.from;
            result.To = source.to;
            result.MessageText = source.body;

            result.SenderId = source.type == message.typeEnum.groupchat ? GetQbUserIdFromGroupJid(source.from) : GetQbUserIdFromJid(source.from);
            result.IsTyping = source.Element(XMPP.tags.jabber.protocol.chatstates.Namespace.composing) != null;
            result.IsPausedTyping = source.Element(XMPP.tags.jabber.protocol.chatstates.Namespace.paused) != null;
        }

        private void FillExtraParamsFields(message source, Message result)
        {
            var extraParams = source.Element(ExtraParams.XName);
            if (extraParams != null)
            {
                var dialogId = extraParams.Element(DialogId.XName);
                if (dialogId != null) result.ChatDialogId = dialogId.Value;

                var dateSent = extraParams.Element(DateSent.XName);
                if (dateSent != null)
                {
                    long longValue;
                    if (long.TryParse(dateSent.Value, out longValue))
                    {
                        result.DateSent = longValue;
                    }
                }

                var notificationType = extraParams.Element(NotificationType.XName);
                if (notificationType != null)
                {
                    int intValue;
                    if (int.TryParse(notificationType.Value, out intValue))
                    {
                        if (Enum.IsDefined(typeof(NotificationTypes), intValue))
                            result.NotificationType = (NotificationTypes)intValue;
                    }
                }

                var roomPhoto = extraParams.Element(RoomPhoto.XName);
                if (roomPhoto != null)
                {
                    result.RoomPhoto = roomPhoto.Value;
                }

                var roomName = extraParams.Element(RoomName.XName);
                if (roomName != null)
                {
                    result.RoomName = roomName.Value;
                }

                var occupantsIds = extraParams.Element(OccupantsIds.XName);
                if (occupantsIds != null)
                {
                    result.OccupantsIds = occupantsIds.Value;
                }
            }
        }

        private void OnPresence(presence presence)
        {
            var receivedPresence = new Presence
            {
                From = presence.from,
                To = presence.to,
                PresenceType = (PresenceType) presence.type
            };

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
                        int userId = GetQbUserIdFromJid(item.jid);
                        if (userId == 0) continue;

                        Contacts.RemoveAll(c => c.UserId == userId);

                        if (item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.both
                            || item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.from
                            || item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.to)
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

        #region Working with JIDs

        private string BuildJid(int userId)
        {
            return string.Format("{0}-{1}@{2}", userId, ApplicationId, ChatEndpoint);
        }

        private int GetQbUserIdFromJid(string jid)
        {
            var match = qbJidRegex.Match(jid);

            if (!match.Success || match.Groups.Count == 0) return 0;
            int userId;
            if (!int.TryParse(match.Groups[1].Value, out userId)) return 0;
            return userId;
        }

        private int GetQbUserIdFromGroupJid(string groupJid)
        {
            int senderId;
            var jidParts = groupJid.Split('/');
            if (int.TryParse(jidParts.Last(), out senderId))
                return senderId;

            return 0;
        }

        #endregion

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
