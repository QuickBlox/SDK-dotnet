using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Converters;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Modules.ChatModule.Models;
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
    /// ChatXmpp module allows users to chat with each other in private or group dialogs via XMPP protocol.
    /// </summary>
    //TODO: refactor this. Move contacts functionality to some ContactManager, Presences to some PresenceManager
    public class ChatXmppClient : IChatXmppClient, IRosterManager
    {
        private enum JidType
        {
            /// <summary>
            /// Type is unknown
            /// </summary>
            Unknown,
            /// <summary>
            /// User's JID
            /// </summary>
            Private,
            /// <summary>
            /// User's JID in some group chat
            /// </summary>
            Group
        }

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
        public event EventHandler<Message> MessageReceived;

        /// <summary>
        /// Event occuring when a new message is received.
        /// </summary>
        public event EventHandler<SystemMessage> SystemMessageReceived;

        /// <summary>
        /// Event occuring when a new message has been sent by the current user but from a different device (Message carbons)
        /// </summary>
        public event EventHandler<Message> MessageSent;

        /// <summary>
        /// Event occuring when a new message has been sent by the current user but from a different device (Message carbons)
        /// </summary>
        public event EventHandler<SystemMessage> SystemMessageSent;

        /// <summary>
        /// Event occuring  when a presence is received.
        /// </summary>
        public event EventHandler<Presence> PresenceReceived;

        /// <summary>
        /// Event occuring  when your contacts in roster have changed.
        /// </summary>
        public event EventHandler ContactsChanged;

        /// <summary>
        /// Event occuring when a contact is added to contact list.
        /// </summary>
        public event EventHandler<Contact> ContactAdded;

        /// <summary>
        /// Event occuring when a contact is removed from contact list.
        /// </summary>
        public event EventHandler<Contact> ContactRemoved;

        /// <summary>
        /// Event occuring when xmpp connection is lost.
        /// </summary>
        public event EventHandler Disconnected;

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
        /// <param name="userId">User ID</param>
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

        /// <summary>
        /// Enables Message Carbons which allows to have sync conversations in case a user has several devices.
        /// </summary>
        public void EnableMessageCarbons()
        {
            var carbonsEnable = new MessageCarbonsEnable();
            iq iq = new iq { type = iq.typeEnum.set, id="enable1" };
            iq.Add(carbonsEnable);

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

                var handler = Disconnected;
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
            var messageCarbonMessageSent = msg.Element(MessageCarbonsMessageSent.XName);
            var forwardedMessage = messageCarbonMessageSent?.Element(ForwardedMessage.XName);
            var messageSent = forwardedMessage?.Element(XMPP.tags.jabber.client.Namespace.message);

            if (messageSent != null)
                {
                if (CheckIsSystemMessage(messageSent))
                {
                    OnSystemMessageSent(messageSent);
                }
                else
                {
                    OnUsualMessageSent(messageSent);   
            }
            }
            else
            {
                if (CheckIsSystemMessage(msg))
                {
                    OnSystemMessage(msg);
                }
                else
                {
                    OnUsualMessage(msg);
                }
            }
        }

        #region Usual messages

        private void OnUsualMessage(message msg)
        {
                var receivedMessage = new Message();

            FillUsualMessageFields(msg, receivedMessage);
            FillUsualMessageExtraParamsFields(msg, receivedMessage);
            FillAttachments(msg, receivedMessage);

            MessageReceived?.Invoke(this, receivedMessage);
            }

        private void OnUsualMessageSent(XMPP.tags.Tag msg)
        {
            var receivedMessage = new Message();

            FillUsualMessageFields(msg, receivedMessage);
            FillUsualMessageExtraParamsFields(msg, receivedMessage);

            MessageSent?.Invoke(this, receivedMessage);
                    }

        private void FillUsualMessageFields(XMPP.tags.Tag source, Message result)
        {
            string from = source.GetAttributeValue(XName.Get("from")).ToString();
            string to = source.GetAttributeValue(XName.Get("to")).ToString();
            string type = source.GetAttributeValue(XName.Get("type")).ToString();

            result.From = from;
            result.To = to;
            result.MessageText = source.Element(XName.Get("body", "jabber:client"))?.Value;

            result.SenderId = type == message.typeEnum.groupchat.ToString() ? GetQbUserIdFromGroupJid(from) : GetQbUserIdFromJid(from);
            result.IsTyping = source.Element(XMPP.tags.jabber.protocol.chatstates.Namespace.composing) != null;
            result.IsPausedTyping = source.Element(XMPP.tags.jabber.protocol.chatstates.Namespace.paused) != null;
        }

        private void FillUsualMessageExtraParamsFields(XMPP.tags.Tag source, Message result)
        {
            var extraParams = source.Element(ExtraParams.XName);
            if (extraParams != null)
            {
                result.ChatDialogId = GetExtraParam(extraParams, ExtraParamsList.dialog_id);

                var dateSent = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.date_sent));
                if (dateSent != null)
                {
                    long longValue;
                    if (long.TryParse(dateSent.Value, out longValue))
                    {
                        result.DateSent = longValue;
                    }
                }

                var stringIntListConverter = new StringIntListConverter();

                result.NotificationType = GetNotificationType(extraParams);

                result.RoomPhoto = GetExtraParam(extraParams, ExtraParamsList.room_photo);
                result.RoomName = GetExtraParam(extraParams, ExtraParamsList.room_name);
                result.OccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.occupants_ids));
                result.CurrentOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.current_occupant_ids));
                result.AddedOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.added_occupant_ids));
                result.DeletedOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.deleted_occupant_ids));

                double roomUpdateDate;
                if (Double.TryParse(GetExtraParam(extraParams, ExtraParamsList.room_updated_date), out roomUpdateDate))
                {
                    result.RoomUpdateDate = roomUpdateDate;
                }
            
                var deletedId = GetExtraParam(extraParams, ExtraParamsList.deleted_id);
                if (deletedId != null)
                {
                    int deletedIdInt;
                    if (int.TryParse(deletedId, out deletedIdInt))
                        result.DeletedId = deletedIdInt;
                }
            }
        }

        #endregion

        #region System messages

        private bool CheckIsSystemMessage(XMPP.tags.Tag msg)
        {
            var msgType = msg.GetAttributeValue(XName.Get("type")).ToString();
            if (msgType != message.typeEnum.headline.ToString())
                return false;

            var extraParams = msg.Element(ExtraParams.XName);
            var moduleIdentifier = extraParams?.Element(ExtraParams.GetXNameFor(ExtraParamsList.moduleIdentifier));

            if (moduleIdentifier == null || moduleIdentifier.Value != SystemMessage.SystemMessageModuleIdentifier)
                return false;

            return true;
        }

        private void OnSystemMessage(XMPP.tags.Tag msg)
        {
            var extraParams = msg.Element(ExtraParams.XName);
            var notificationType = GetNotificationType(extraParams);
            if (notificationType == NotificationTypes.GroupCreate || notificationType == NotificationTypes.GroupUpdate)
            {
                var groupInfoMessage = new GroupInfoMessage();
                FillGroupInfoMessageFields(msg, groupInfoMessage);
                SystemMessageReceived?.Invoke(this, groupInfoMessage);
            }
        }

        private void OnSystemMessageSent(XMPP.tags.Tag msg)
        {
            var extraParams = msg.Element(ExtraParams.XName);
            var notificationType = GetNotificationType(extraParams);
            if (notificationType == NotificationTypes.GroupCreate || notificationType == NotificationTypes.GroupUpdate)
            {
                var groupInfoMessage = new GroupInfoMessage();
                FillGroupInfoMessageFields(msg, groupInfoMessage);
                SystemMessageSent?.Invoke(this, groupInfoMessage);
            }
        }

        private void FillGroupInfoMessageFields(XMPP.tags.Tag source, GroupInfoMessage groupInfoMessage)
        {
            var extraParams = source.Element(ExtraParams.XName);
            var stringIntListConverter = new StringIntListConverter();

            groupInfoMessage.DialogId = GetExtraParam(extraParams, ExtraParamsList.dialog_id);
            groupInfoMessage.RoomName = GetExtraParam(extraParams, ExtraParamsList.room_name);
            groupInfoMessage.RoomPhoto = GetExtraParam(extraParams, ExtraParamsList.room_photo);
            groupInfoMessage.DateSent = GetDateTimeExtraParam(extraParams, ExtraParamsList.date_sent);
            groupInfoMessage.RoomUpdatedDate = GetDateTimeExtraParam(extraParams, ExtraParamsList.room_updated_date);
            groupInfoMessage.CurrentOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.current_occupant_ids)).ToArray();

            var dialogType = GetExtraParam(extraParams, ExtraParamsList.type);
            if (dialogType != null)
            {
                int intValue;
                if (int.TryParse(dialogType, out intValue) && Enum.IsDefined(typeof(DialogType), intValue))
                {
                    groupInfoMessage.DialogType = (DialogType)intValue;
                }
            }
        }

        private void FillAttachments(message source, Message result)
        {
            var extraParams = source.Element(ExtraParams.XName);
            if (extraParams != null)
            {
                var attachmentParam = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.attachment));
                if (attachmentParam != null)
                {
                    var attachemnt = new Attachment
                    {
                        Name = attachmentParam.GetAttributeValue(XName.Get("name"))?.ToString(),
                        Id = attachmentParam.GetAttributeValue(XName.Get("id"))?.ToString(),
                        Type = attachmentParam.GetAttributeValue(XName.Get("type"))?.ToString(),
                        Url = attachmentParam.GetAttributeValue(XName.Get("url"))?.ToString()
                    };

                    result.Attachments = new Attachment[] {attachemnt};
                }
            }
        }

        #endregion

        #region Working with ExtraParams

        private NotificationTypes GetNotificationType(XElement extraParams)
        {
            var notificationType = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.notification_type));
            if (notificationType != null)
            {
                int intValue;
                if (int.TryParse(notificationType.Value, out intValue))
                {
                    if (Enum.IsDefined(typeof(NotificationTypes), intValue))
                        return (NotificationTypes)intValue;
                }
            }

            return default(NotificationTypes);
        }

        private string GetExtraParam(XElement extraParams, ExtraParamsList neededExtraParam)
        {
            var extraParam = extraParams.Element(ExtraParams.GetXNameFor(neededExtraParam));
            return extraParam?.Value;
        }

        private DateTime GetDateTimeExtraParam(XElement extraParams, ExtraParamsList neededExtraParam)
        {
            var dateTimeExtraParam = GetExtraParam(extraParams, neededExtraParam);
            if (dateTimeExtraParam != null)
            {
                long longValue;
                if (long.TryParse(dateTimeExtraParam, out longValue))
                {
                    return longValue.ToDateTime();
                }
            }

            return default(DateTime);
        }

        #endregion

        #region Presences and IQ

        private void OnPresence(presence presence)
        {
            var fromJidType = DetermineJidType(presence.from);

            if (fromJidType == JidType.Private)
            {
                OnPrivatePresence(presence);
            }
        }

        private void OnPrivatePresence(presence presence)
        {
            var receivedPresence = new Presence
            {
                UserId = GetQbUserIdFromJid(presence.@from),
                PresenceType = (PresenceType) presence.type
            };

            Presences.RemoveAll(p => p.UserId == receivedPresence.UserId);
            Presences.Add(receivedPresence);

            PresenceReceived?.Invoke(this, receivedPresence);
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

                        var contact = new Contact { Name = item.name, UserId = userId };

                        if (item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.both
                            || item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.from
                            || item.subscription == XMPP.tags.jabber.iq.roster.item.subscriptionEnum.to)
                        {
                            Contacts.Add(contact);

                            if (iq.type == iq.typeEnum.set)
                            {
                                ContactAdded?.Invoke(this, contact);
                            }
                        }

                        if (item.subscription == item.subscriptionEnum.remove && iq.type == iq.typeEnum.set)
                        {
                            ContactRemoved?.Invoke(this, contact);
                        }
                    }

                    ContactsChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion

        #region Working with JIDs

        private string BuildJid(int userId)
        {
            return $"{userId}-{ApplicationId}@{ChatEndpoint}";
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

        private JidType DetermineJidType(string jid)
        {
            if(string.IsNullOrEmpty(jid))
                return JidType.Unknown;

            var jidParts = jid.Split('@');

            if (jidParts.Length != 2) // userPart@serverPart
                return JidType.Unknown;

            var serverPart = jidParts[1];

            if(serverPart.StartsWith(quickbloxClient.MucChatEndpoint))
                return JidType.Group;

            if(serverPart.StartsWith(quickbloxClient.ChatEndpoint))
                return JidType.Private;

            return JidType.Unknown;
        }

        #endregion

        #endregion

    }

}
