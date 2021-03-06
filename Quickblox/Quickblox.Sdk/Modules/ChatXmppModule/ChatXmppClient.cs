﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Diagnostics;
using System.Linq;
using Xmpp.Client;
using Quickblox.Sdk.GeneralDataModel.Models;
using System.Xml.Linq;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Converters;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Xmpp.Im;
using Xmpp;
using Xmpp.Extensions;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class ChatXmppClient
    {
        #region Fields

        private QuickbloxClient quickbloxClient;
        private XmppClient xmppClient;
        private static readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");
        private const string JidPattern = "{0}-{1}@{2}";
        private const string SmallJidPattern = "{0}-{1}";
        private int userId;

        #endregion
        
        #region Ctor

        public ChatXmppClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
            Contacts = new List<RosterItem>();
            Presences = new List<Jid>();
        }

        #endregion

        #region Properties
        
        public bool IsConnected
        {
            get
            {
                return xmppClient != null && xmppClient.Connected;
            }
        }

        public Jid MyJid {
            get
            {
                if (quickbloxClient == null)
                {
                    throw new ArgumentNullException("quickbloxClient");
                }
                return BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            }
        }

        public Jid MySmallJid
        {
            get
            {
                if (quickbloxClient == null)
                {
                    throw new ArgumentNullException("quickbloxClient");
                }
                return BuildSmallJid(userId, quickbloxClient.ApplicationId);
            }
        }

        public List<RosterItem> Contacts { get; private set; }

        public List<Jid> Presences { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a new Message is received.
        /// </summary>
        public event MessageEventHandler MessageReceived;

        /// <summary>
        /// Event occuring when a new Message has been sent by the current user but from a different device (Message carbons)
        /// </summary>
        //public event MessageEventHandler MessageSent;

        /// <summary>
        /// Occurs when a new system Message is received.
        /// </summary>
        public event SystemMessageEventHandler SystemMessageReceived;

        /// <summary>
        /// Event occuring when a new Message has been sent by the current user but from a different device (Message carbons)
        /// </summary>
        //public event SystemMessageEventHandler SystemMessageSent;

        /// <summary>
        /// Occurs when a new error is received.
        /// </summary>
        public event ErrorsEventHandler ErrorReceived;

        /// <summary>
        /// Occurs when a activity is changed.
        /// </summary>
        public event ActivityChangedEventHandler ActivityChanged;

        /// <summary>
        /// Occurs when a new chatState is changed.
        /// </summary>
        public event ChatStateChangedEventHandler ChatStateChanged;

        /// <summary>
        /// Occurs when a moodState is changed.
        /// </summary>
        public event MoodChangedEventHandler MoodStateChanged;

        /// <summary>
        /// Occurs when a roster is changed.
        /// </summary>
        public event RosterUpdatedEventHandler RosterUpdated;

        /// <summary>
        /// Occurs when a status is changed.
        /// </summary>
        public event StatusEventHandler StatusChanged;

        /// <summary>
        /// Occurs when a subscriptions is changed.
        /// </summary>
        public event SubscriptionsEventHandler SubscriptionsChanged;

        /// <summary>
        /// Occurs when a new Message is received.
        /// </summary>
        public event TuneEventHandler Tune;

        private void UnSubcribeEvents(XmppClient xmppClient)
        {
            xmppClient.Error -= OnClientErrorCallback;
            xmppClient.Message -= OnMessageReceivedCallback;
            xmppClient.ActivityChanged -= OnActivityChangedCallback;
            xmppClient.ChatStateChanged -= OnChatStateChangedCallback;
            xmppClient.MoodChanged -= OnMoodChangedCallback;
            xmppClient.RosterUpdated -= OnRosterUpdatedCallback;
            xmppClient.StatusChanged -= OnStatusChangedCallback;
            xmppClient.SubscriptionApproved -= OnSubscriptionApprovedCallback;
            xmppClient.SubscriptionRefused -= OnSubscriptionRefusedCallback;
            xmppClient.Unsubscribed -= OnUnsubscribedCallback;
            xmppClient.Tune -= OnTuneCallback;
        }

        private void SubscribeEvents(XmppClient xmppClient)
        {
            xmppClient.Error += OnClientErrorCallback;
            xmppClient.Message += OnMessageReceivedCallback;
            xmppClient.ActivityChanged += OnActivityChangedCallback;
            xmppClient.ChatStateChanged += OnChatStateChangedCallback;
            xmppClient.MoodChanged += OnMoodChangedCallback;
            xmppClient.RosterUpdated += OnRosterUpdatedCallback;
            xmppClient.StatusChanged += OnStatusChangedCallback;
            xmppClient.SubscriptionApproved += OnSubscriptionApprovedCallback;
            xmppClient.SubscriptionRefused += OnSubscriptionRefusedCallback;
            xmppClient.Unsubscribed += OnUnsubscribedCallback;
            xmppClient.Tune += OnTuneCallback;
        }

        #endregion

        #region Callbacks

        private async void OnTuneCallback (object sender, Xmpp.Extensions.TuneEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnTune:");
            var handler = Tune;
            if (handler != null)
            {
                var jid = new Jid(e.Jid.ToString());
                TuneInformation information = null;
                if (e.Information != null)
                {
                    information = new TuneInformation(e.Information.Title, e.Information.Artist, e.Information.Track, e.Information.Length, e.Information.Rating, e.Information.Source, e.Information.Uri);
                }

                handler.Invoke(this, new TuneEventArgs(jid, information));
            }

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> ============///////////////////////==============");
        }

        private async void OnUnsubscribedCallback (object sender, UnsubscribedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnUnsubscribed:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(e.Jid, PresenceType.Unsubscribed));
            }
        }

        private async void OnSubscriptionRefusedCallback (object sender, Xmpp.Im.SubscriptionRefusedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnSubscriptionRefused:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(e.Jid, PresenceType.Refused));
            }
        }

        private async void OnSubscriptionApprovedCallback (object sender, Xmpp.Im.SubscriptionApprovedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnSubscriptionApproved:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(e.Jid, PresenceType.Subscribed));
            }
        }

        private async void OnStatusChangedCallback (object sender, Xmpp.Im.StatusEventArgs e)
		{
            if (MyJid == null)
                return;

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnStatusChanged:" + e.Jid.ToString() + " Status: " + e.Status.Availability);
            
            if (e.Jid != new Xmpp.Jid(MyJid.ToString()))
            {
                var jid = e.Jid.ToString();
                if (e.Status.Availability == Xmpp.Im.Availability.Online)
                {
                    if (!this.Presences.Contains(jid))
                    {
                        await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> Added to Presences. Jid: " + e.Jid);
                        this.Presences.Add(jid);
                    }
                }
                else
                {
                    await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> Removed from Presences. Jid: " + e.Jid);
                    this.Presences.Remove(jid);
                }
            }

            var handler = StatusChanged;
            if (handler != null)
            {
                var availability = (Availability)Enum.Parse(typeof(Availability), e.Status.Availability.ToString());
                var status = new Status(availability, e.Status.Messages, e.Status.Priority);
                handler.Invoke(this, new StatusEventArgs(e.Jid, status));
            }
        }

        private async void OnRosterUpdatedCallback (object sender, Xmpp.Im.RosterUpdatedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnRosterUpdated:" +
                                                    " IsRemoved: " + e.Removed +
                                                     " Jid: " + e.Item.Jid + 
                                                     " Name: " + e.Item.Name + 
                                                     " SubscriptionState: " + e.Item.SubscriptionState);

            var handler = RosterUpdated;
            if (handler != null)
            {
                handler.Invoke(this, new RosterUpdatedEventArgs(e.Item, e.Removed));
            }
        }

        private async void OnMoodChangedCallback (object sender, Xmpp.Extensions.MoodChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnMoodChanged:" +
                                                    " Description: " + e.Description +
                                                     " Jid: " + e.Jid + " Mood state: " + e.Mood);

            var handler = MoodStateChanged;
            if (handler != null)
            {
                handler.Invoke(this, new MoodChangedEventArgs(new Jid(e.Jid.ToString()), e.Mood, e.Description));
            }
        }

        private async void OnChatStateChangedCallback (object sender, Xmpp.Extensions.ChatStateChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnChatStateChanged:" +
                                                    " Jid: " + e.Jid +
                                                    " ChatState: " + e.ChatState.ToString());
            var handler = ChatStateChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ChatStateChangedEventArgs(e.Jid, e.ChatState));
            }
        }

        private async void OnActivityChangedCallback (object sender, Xmpp.Extensions.ActivityChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnActivityChanged:" +
                                                    " Jid: " + e.Jid +
                                                    " ChatState: " + e.Activity.ToString());

            Debug.WriteLine("XMPP: ====>  OnActivityChanged:");
            var handler = ActivityChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ActivityChangedEventArgs(e.Jid, e.Activity.ToString(), e.Specific.ToString(), e.Description));
            }
        }

        private async void OnClientErrorCallback(object sender, Xmpp.Im.ErrorEventArgs args)
        {
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnClientError:" +
                                                    " Exception: " + args.ToString());
            
            var handler = ErrorReceived;
            if (handler != null)
            {
                handler.Invoke(sender, new ErrorEventArgs(args.Exception));
            }
        }

        private async void OnMessageReceivedCallback(object sender, Xmpp.Im.MessageEventArgs messageEventArgs)
        {
            //var element = XElement.Parse(messageEventArgs.Message.DataString);
            //var mesageCardbonMessageSent = element.Element(MessageCarbonsMessageSent.XName);
            //XElement forwardedMessage = null;
            //if (mesageCardbonMessageSent != null)
            //{
            //    forwardedMessage = mesageCardbonMessageSent.Element(ForwardedMessage.XName);
            //}

            //if (forwardedMessage != null)
            //{
            //    if (CheckIsSystemMessage(messageEventArgs.Message))
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            {
                if (CheckIsSystemMessage(messageEventArgs.xmppMessage))
                {
                    OnSystemMessage(messageEventArgs.xmppMessage);
                }
                else
                {
                    OnUsualMessage(messageEventArgs.xmppMessage);
                }
            }
        }

        private bool CheckIsSystemMessage(Xmpp.Im.Message xmppXmppMessage)
        {
            if (xmppXmppMessage.Type != Xmpp.Im.MessageType.Headline)
                return false;
            
            if (xmppXmppMessage.ExtraParameters != null)
            {
                var moduleIdentifier = xmppXmppMessage.ExtraParameters.Element(ExtraParams.GetXNameFor(ExtraParamsList.moduleIdentifier));
                if (moduleIdentifier == null || moduleIdentifier.Value != SystemMessage.SystemMessageModuleIdentifier)
                    return false;
            }

            return true;
        }

        private async void OnUsualMessage(Xmpp.Im.Message xmppXmppMessage)
        {
            var receivedMessage = new GeneralDataModel.Models.Message();
            FillUsualMessageFields(xmppXmppMessage, receivedMessage);

            FillUsualMessageExtraParamsFields(xmppXmppMessage.ExtraParameters, receivedMessage);
            FillAttachments(xmppXmppMessage.ExtraParameters, receivedMessage);

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: OnMessageReceived ====> " +
                            " From: " + receivedMessage.SenderId +
                            " To: " + receivedMessage.RecipientId +
                            " Body: " + receivedMessage.MessageText +
                            " DateSent " + receivedMessage.DateSent +
                            " FullXmlMessage: " + xmppXmppMessage.DataString);

            receivedMessage.ExtraParameters = xmppXmppMessage.ExtraParameters;

            var handler = MessageReceived;
            if(handler != null)
            {
                var messageEventArgs = new MessageEventArgs(xmppXmppMessage.From, receivedMessage, xmppXmppMessage.Type);
                handler.Invoke(this, messageEventArgs);
            }
        }
        
        private async void OnSystemMessage(Xmpp.Im.Message xmppXmppMessage)
        {
            var notificationType = GetNotificationType(xmppXmppMessage.ExtraParameters);
            SystemMessage systemMessage = null;
            if (notificationType == NotificationTypes.GroupCreate || notificationType == NotificationTypes.GroupUpdate)
            {
                systemMessage = new GroupInfoMessage();
                FillSystemMessageFields(xmppXmppMessage, systemMessage);
                FillGroupInfoMessageFields(xmppXmppMessage.ExtraParameters, (GroupInfoMessage)systemMessage);
            }
            else
            {
                systemMessage = new SystemMessage();
                FillSystemMessageFields(xmppXmppMessage, systemMessage);
            }

            systemMessage.ExtraParameters = xmppXmppMessage.ExtraParameters;

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: OnMessageReceived ====> " +
                           " From: " + systemMessage.SenderId +
                           " Body: " + systemMessage.MessageText +
                           " FullXmlMessage: " + xmppXmppMessage.DataString);

            var handler = SystemMessageReceived;
            if (handler != null)
            {
                var systemMessageEventArgs = new SystemMessageEventArgs(new Jid(xmppXmppMessage.From.ToString()), systemMessage, xmppXmppMessage.Type);
                handler.Invoke(this, systemMessageEventArgs);
            }
        }

        private void FillSystemMessageFields(Xmpp.Im.Message xmppXmppMessage, SystemMessage result)
        {
            string from = xmppXmppMessage.From.ToString();
            string to = xmppXmppMessage.To.ToString();

            result.From = from;
            result.To = to;
            result.MessageText = xmppXmppMessage.Body;

            result.SenderId = xmppXmppMessage.Type == Xmpp.Im.MessageType.Groupchat ? GetQbUserIdFromGroupJid(from) : GetQbUserIdFromJid(from);
        }

        private void FillUsualMessageFields(Xmpp.Im.Message xmppXmppMessage, GeneralDataModel.Models.Message result)
        {
            string from = xmppXmppMessage.From.ToString();
            string to = xmppXmppMessage.To.ToString();

            result.From = from;
            result.To = to;
            result.MessageText = xmppXmppMessage.Body;

            result.Id = xmppXmppMessage.Id;
            result.RecipientId = xmppXmppMessage.Type == Xmpp.Im.MessageType.Groupchat ? GetQbUserIdFromGroupJid(to) : GetQbUserIdFromJid(to);
            result.SenderId = xmppXmppMessage.Type == Xmpp.Im.MessageType.Groupchat ? GetQbUserIdFromGroupJid(from) : GetQbUserIdFromJid(from);
        }

        private void FillUsualMessageExtraParamsFields(XElement extraParams, GeneralDataModel.Models.Message result)
        {
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
                result.CurrentOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.current_occupant_ids));
                result.AddedOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.added_occupant_ids));
                result.DeletedOccupantsIds = stringIntListConverter.ConvertToIntList(GetExtraParam(extraParams, ExtraParamsList.deleted_occupant_ids));
                result.ExtraParameters = extraParams;

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

        private void FillGroupInfoMessageFields(XElement extraParams, GroupInfoMessage groupInfoMessage)
        {
            groupInfoMessage.DialogId = GetExtraParam(extraParams, ExtraParamsList.dialog_id);
            groupInfoMessage.RoomName = GetExtraParam(extraParams, ExtraParamsList.room_name);
            groupInfoMessage.RoomPhoto = GetExtraParam(extraParams, ExtraParamsList.room_photo);
            groupInfoMessage.DateSent = GetDateTimeExtraParam(extraParams, ExtraParamsList.date_sent);
            groupInfoMessage.RoomUpdatedDate = GetDateTimeExtraParam(extraParams, ExtraParamsList.room_updated_date);

            StringIntListConverter stringIntListConverter = new StringIntListConverter();
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

        private void FillAttachments(XElement extraParams, GeneralDataModel.Models.Message result)
        {
            if (extraParams != null)
            {
                var attachmentParam = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.attachment));
                if (attachmentParam != null)
                {
                    var attachemnt = new Attachment
                    {
                        Name = GetAttributeValue(attachmentParam, XName.Get("name")),
                        Id = GetAttributeValue(attachmentParam, XName.Get("id")),
                        Type = GetAttributeValue(attachmentParam, XName.Get("type")),
                        Url = GetAttributeValue(attachmentParam, XName.Get("url"))
                    };

                    result.Attachments = new Attachment[] { attachemnt };
                }
            }
        }
        
        #endregion

        #region Public methods

        public void SendMessage(int userId, string body, XElement extraParams = null, string dialogId = null, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var messageId = MongoObjectIdGenerator.GetNewObjectIdString();
            var wrappedMessageType = (Xmpp.Im.MessageType)Enum.Parse(typeof(Xmpp.Im.MessageType), messageType.ToString());
            var jidString = BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            var jid = new Xmpp.Jid(jidString);
            var message = xmppClient.SendMessage(jid, messageId, body, subject, dialogId, extraParams, wrappedMessageType);

            LoggerHolder.Log(LogLevel.Debug, "XMPP: SentMessage ====> " + message.DataString);
        }

        public void SendMessage(string jid, string body, XElement extraParams = null, string dialogId = null, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var messageId = MongoObjectIdGenerator.GetNewObjectIdString();
            var message = xmppClient.SendMessage(jid, messageId, body, subject, dialogId, extraParams, messageType);

            LoggerHolder.Log(LogLevel.Debug, "XMPP: SentMessage ====> " + message.DataString);
        }

        public void ReloadContacts()
        {
            this.Contacts.Clear();
            var roster = xmppClient.GetRoster();
            foreach (var item in roster)
            {
                this.Contacts.Add(item);
            }
        }

        public void SetChatState(string otherUserJid, ChatState chatState)
        {
            xmppClient.SetChatState(new Jid(otherUserJid), chatState);
        }

        public void SetSubscribtionStatus(string otherUserJid, SubscriptionMessageType chatState)
        {
            switch (chatState)
            {
                case SubscriptionMessageType.RequestSubscription:
                    xmppClient.RequestSubscription(new Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.ApproveSubscription:
                    xmppClient.ApproveSubscriptionRequest(new Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.RefuseSubscription:
                    xmppClient.RefuseSubscriptionRequest(new Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.RevokeSubscription:
                    xmppClient.RevokeSubscription(new Xmpp.Jid(otherUserJid));
                    break;
                default:
                    break;
            }
        }

        public void SetActivity(GeneralActivity activity, String description)
        {
            xmppClient.SetActivity(activity, description: description);
        }
        
        public void Unblock(int userId)
        {
            xmppClient.Unblock(new Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public IEnumerable<Jid> GetBlocklist()
        {
            return xmppClient.GetBlocklist().Select(baseJid => new Jid(baseJid.ToString()));
        }

        public void Buzz(int userId)
        {
            xmppClient.Buzz(new Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public void BlockUser(int userId)
        {
            xmppClient.Block(new Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }


        public void AddContact(RosterItem user)
        {
			xmppClient.AddContact(new Xmpp.Jid(user.Jid.ToString()), user.Name, user.Groups != null ? user.Groups.ToArray() : null);
        }

        public void RemoveContact(int userId)
        {
            xmppClient.RemoveContact(new Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public void JoinToGroup(string groupJid, string nickName)
        {
            string fullJid = string.Format("{0}/{1}", groupJid, nickName);
            xmppClient.JoinToGroup(new Xmpp.Jid(fullJid), new Xmpp.Jid(quickbloxClient.ChatXmppClient.MyJid.ToString()));
        }

		public void LeaveGroup(string groupJid, string nickName)
		{
			string fullJid = string.Format("{0}/{1}", groupJid, nickName);
			xmppClient.LeaveGroup(new Xmpp.Jid(fullJid), new Xmpp.Jid(quickbloxClient.ChatXmppClient.MyJid.ToString()));
		}

        public TimeSpan Ping(string jid)
        {
			return xmppClient.Ping(new Jid(jid));
        }

        #endregion

        #region Completed

        public async Task<Roster> Connect(int userId, string password)
        {
            Contacts = new List<RosterItem>();
            Presences = new List<Jid>();
            this.userId = userId;

            xmppClient = new XmppClient(quickbloxClient.ChatEndpoint, MySmallJid.ToString(), password);
            xmppClient.Tls = false;
            xmppClient.Port = 5222;
            xmppClient.Password = password;

            SubscribeEvents(xmppClient);
            return await xmppClient.Connect();
        }

        public void Close()
        {
            UnSubcribeEvents(xmppClient);

            Contacts.Clear();
            Presences.Clear();
            userId = 0;

            xmppClient.Close();
        }

        /// <summary>
        /// Creates a private one-to-one chat manager.
        /// </summary>
        /// <param name="otherUserId">Another user ID</param>
        /// <param name="dialogId">Dialog ID with another user</param>
        /// <returns>PrivateChatManager instance.</returns>
        public PrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId)
        {
            return new PrivateChatManager(quickbloxClient, otherUserId, dialogId);
        }

        /// <summary>
        /// Creates a group chat manager.
        /// </summary>
        /// <param name="groupJid">Group XMPP room JID.</param>
        /// <param name="dialogId">Group dialog ID.</param>
        /// <returns>GroupChatManager</returns>
        public GroupChatManager GetGroupChatManager(string groupJid, string dialogId)
        {
            return new GroupChatManager(quickbloxClient, groupJid, dialogId);
        }

        #endregion

        #region Helpers

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

        public string GetAttributeValue(XElement element, XName name)
        {
            XAttribute attr = element.Attribute(name);
            if (attr != null)
                return attr.Value;
            else
                return null;
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

        public static string BuildJid(int userId, int appId, string chatEndpoint)
        {
            var jid = string.Format(JidPattern, userId, appId, chatEndpoint);
            return jid;
        }

        public static string BuildSmallJid(int userId, int appId)
        {
            var jid = string.Format(SmallJidPattern, userId, appId);
            return jid;
        }

        public static int GetQbUserIdFromJid(string jid)
        {
            var match = qbJidRegex.Match(jid);

            if (!match.Success || match.Groups.Count == 0) return 0;
            int userId;
            if (!int.TryParse(match.Groups[1].Value, out userId)) return 0;
            return userId;
        }

        private static int GetQbUserIdFromGroupJid(string groupJid)
        {
            int senderId;
            var jidParts = groupJid.Split('/');
            if (int.TryParse(jidParts.Last(), out senderId))
                return senderId;

            return 0;
        }

        private JidType DetermineJidType(string jid)
        {
            if (string.IsNullOrEmpty(jid))
                return JidType.Unknown;

            var jidParts = jid.Split('@');

            if (jidParts.Length != 2) // userPart@serverPart
                return JidType.Unknown;

            var serverPart = jidParts[1];

            if (serverPart.StartsWith(quickbloxClient.MucChatEndpoint))
                return JidType.Group;

            if (serverPart.StartsWith(quickbloxClient.ChatEndpoint))
                return JidType.Private;

            return JidType.Unknown;
        }

        #endregion
    }
}
