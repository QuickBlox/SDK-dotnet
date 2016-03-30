using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Sharp.Xmpp.Client;
using Quickblox.Sdk.GeneralDataModel.Models;
using System.Threading;
using System.Xml.Linq;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Converters;
using Quickblox.Sdk.Modules.ChatModule.Models;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class ChatXmppClient
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;
        private static readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");
        private const string JidPattern = "{0}-{1}@{2}";
        private const string SmallJidPattern = "{0}-{1}";
        private int userId;

        #endregion
        
        #region Ctor

        public ChatXmppClient(IQuickbloxClient quickbloxClient)
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
        /// Occurs when a new message is received.
        /// </summary>
        public event MessageEventHandler MessageReceived;

        /// <summary>
        /// Event occuring when a new message has been sent by the current user but from a different device (Message carbons)
        /// </summary>
        //public event MessageEventHandler MessageSent;

        /// <summary>
        /// Occurs when a new system message is received.
        /// </summary>
        public event SystemMessageEventHandler SystemMessageReceived;

        /// <summary>
        /// Event occuring when a new message has been sent by the current user but from a different device (Message carbons)
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
        /// Occurs when a new message is received.
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

        private async void OnTuneCallback (object sender, Sharp.Xmpp.Extensions.TuneEventArgs e)
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

        private async void OnUnsubscribedCallback (object sender, Sharp.Xmpp.Im.UnsubscribedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnUnsubscribed:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Unsubscribed));
            }
        }

        private async void OnSubscriptionRefusedCallback (object sender, Sharp.Xmpp.Im.SubscriptionRefusedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnSubscriptionRefused:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Refused));
            }
        }

        private async void OnSubscriptionApprovedCallback (object sender, Sharp.Xmpp.Im.SubscriptionApprovedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnSubscriptionApproved:" + e.Jid.ToString());
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Subscribed));
            }
        }

        private async void OnStatusChangedCallback (object sender, Sharp.Xmpp.Im.StatusEventArgs e)
		{
            if (MyJid == null)
                return;

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnStatusChanged:" + e.Jid.ToString() + " Status: " + e.Status.Availability);
            
            if (e.Jid != new Sharp.Xmpp.Jid(MyJid.ToString()))
            {
                var jid = e.Jid.ToString();
                if (e.Status.Availability == Sharp.Xmpp.Im.Availability.Online)
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
                handler.Invoke(this, new StatusEventArgs(new Jid(e.Jid.ToString()), status));
            }
        }

        private async void OnRosterUpdatedCallback (object sender, Sharp.Xmpp.Im.RosterUpdatedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnRosterUpdated:" +
                                                    " IsRemoved: " + e.Removed +
                                                     " Jid: " + e.Item.Jid + 
                                                     " Name: " + e.Item.Name + 
                                                     " SubscriptionState: " + e.Item.SubscriptionState);

            var handler = RosterUpdated;
            if (handler != null)
            {
                var subscriptionWrappedState = (SubscriptionState)Enum.Parse(typeof(SubscriptionState), e.Item.SubscriptionState.ToString());
                RosterItem wrapper = new RosterItem(e.Item.Jid.Node, e.Item.Name, subscriptionWrappedState, e.Item.Pending, e.Item.Groups);
                handler.Invoke(this, new RosterUpdatedEventArgs(wrapper, e.Removed));
            }
        }

        private async void OnMoodChangedCallback (object sender, Sharp.Xmpp.Extensions.MoodChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnMoodChanged:" +
                                                    " Description: " + e.Description +
                                                     " Jid: " + e.Jid + " Mood state: " + e.Mood);

            var handler = MoodStateChanged;
            if (handler != null)
            {
                var moodeState = (Mood)Enum.Parse(typeof(Mood), e.Mood.ToString());
                handler.Invoke(this, new MoodChangedEventArgs(new Jid(e.Jid.ToString()), moodeState, e.Description));
            }
        }

        private async void OnChatStateChangedCallback (object sender, Sharp.Xmpp.Extensions.ChatStateChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnChatStateChanged:" +
                                                    " Jid: " + e.Jid +
                                                    " ChatState: " + e.ChatState.ToString());
            var handler = ChatStateChanged;
            if (handler != null)
            {
                var chatState = (ChatState)Enum.Parse(typeof(ChatState), e.ChatState.ToString());
                handler.Invoke(this, new ChatStateChangedEventArgs(new Jid(e.Jid.ToString()), chatState));
            }
        }

        private async void OnActivityChangedCallback (object sender, Sharp.Xmpp.Extensions.ActivityChangedEventArgs e)
		{
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnActivityChanged:" +
                                                    " Jid: " + e.Jid +
                                                    " ChatState: " + e.Activity.ToString());

            Debug.WriteLine("XMPP: ====>  OnActivityChanged:");
            var handler = ActivityChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ActivityChangedEventArgs(e.Jid.ToString(), e.Activity.ToString(), e.Specific.ToString(), e.Description));
            }

        }

        private async void OnClientErrorCallback(object sender, Sharp.Xmpp.Im.ErrorEventArgs args)
        {
            await LoggerHolder.Log(LogLevel.Debug, "XMPP: ====> OnClientError:" +
                                                    " Exception: " + args.ToString());
            
            var handler = ErrorReceived;
            if (handler != null)
            {
                handler.Invoke(sender, new ErrorEventArgs(args.Exception));
            }
        }

        private async void OnMessageReceivedCallback(object sender, Sharp.Xmpp.Im.MessageEventArgs messageEventArgs)
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
                if (CheckIsSystemMessage(messageEventArgs.Message))
                {
                    OnSystemMessage(messageEventArgs.Message);
                }
                else
                {
                    OnUsualMessage(messageEventArgs.Message);
                }
            }
        }

        private bool CheckIsSystemMessage(Sharp.Xmpp.Im.Message message)
        {
            if (message.Type != Sharp.Xmpp.Im.MessageType.Headline)
                return false;

            var extraParams = XElement.Parse(message.ExtraParameter);
            if (extraParams != null)
            {
                var moduleIdentifier = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.moduleIdentifier));
                if (moduleIdentifier == null || moduleIdentifier.Value != SystemMessage.SystemMessageModuleIdentifier)
                    return false;
            }

            return true;
        }

        private async void OnUsualMessage(Sharp.Xmpp.Im.Message message)
        {
            var receivedMessage = new Message();
            FillUsualMessageFields(message, receivedMessage);

            var extraParams = XElement.Parse(message.ExtraParameter);
            FillUsualMessageExtraParamsFields(extraParams, receivedMessage);
            FillAttachments(extraParams, receivedMessage);

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: OnMessageReceived ====> " +
                            " From: " + receivedMessage.SenderId +
                            " To: " + receivedMessage.RecipientId +
                            " Body: " + receivedMessage.MessageText +
                            " DateSent " + receivedMessage.DateSent +
                            " FullXmlMessage: " + message.DataString);

            var handler = MessageReceived;
            if(handler != null)
            {
                var wappedMessageType = (MessageType)Enum.Parse(typeof(MessageType), message.Type.ToString());
                var messageEventArgs = new MessageEventArgs(new Jid(message.From.ToString()), receivedMessage, wappedMessageType);
                handler.Invoke(this, messageEventArgs);
            }
        }
        
        private async void OnSystemMessage(Sharp.Xmpp.Im.Message message)
        {
            var extraParams = XElement.Parse(message.ExtraParameter);
            var notificationType = GetNotificationType(extraParams);
            SystemMessage systemMessage = null;
            if (notificationType == NotificationTypes.GroupCreate || notificationType == NotificationTypes.GroupUpdate)
            {
                systemMessage = new GroupInfoMessage();
                FillSystemMessageFields(message, systemMessage);
                FillGroupInfoMessageFields(extraParams, (GroupInfoMessage)systemMessage);
            }
            else
            {
                systemMessage = new SystemMessage();
                FillSystemMessageFields(message, systemMessage);
            }

            await LoggerHolder.Log(LogLevel.Debug, "XMPP: OnMessageReceived ====> " +
                           " From: " + systemMessage.SenderId +
                           " Body: " + systemMessage.MessageText +
                           " DateSent " + systemMessage.DateSent +
                           " FullXmlMessage: " + message.DataString);

            var handler = SystemMessageReceived;
            if (handler != null)
            {
                var wappedMessageType = (MessageType)Enum.Parse(typeof(MessageType), message.Type.ToString());
                var systemMessageEventArgs = new SystemMessageEventArgs(new Jid(message.From.ToString()), systemMessage, wappedMessageType);
                handler.Invoke(this, systemMessageEventArgs);
            }
        }

        private void FillSystemMessageFields(Sharp.Xmpp.Im.Message message, SystemMessage result)
        {
            string from = message.From.ToString();
            string to = message.To.ToString();

            result.From = from;
            result.To = to;
            result.MessageText = message.Body;

            result.SenderId = message.Type == Sharp.Xmpp.Im.MessageType.Groupchat ? GetQbUserIdFromGroupJid(from) : GetQbUserIdFromJid(from);
        }

        private void FillUsualMessageFields(Sharp.Xmpp.Im.Message message, Message result)
        {
            string from = message.From.ToString();
            string to = message.To.ToString();
            var wappedMessageType = (MessageType)Enum.Parse(typeof(MessageType), message.Type.ToString());

            result.From = from;
            result.To = to;
            result.MessageText = message.Body;

            result.SenderId = message.Type == Sharp.Xmpp.Im.MessageType.Groupchat ? GetQbUserIdFromGroupJid(from) : GetQbUserIdFromJid(from);
        }

        private void FillUsualMessageExtraParamsFields(XElement extraParams, Message result)
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

        private void FillAttachments(XElement extraParams, Message result)
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

   //     private async void OnMessageReceivedCallback2(object sender, Sharp.Xmpp.Im.MessageEventArgs messageEventArgs)
   //     {
   //         var receivedMessage = new Message();
   //         receivedMessage.Id = messageEventArgs.Message.Id;
   //         receivedMessage.From = messageEventArgs.Message.From.ToString();
   //         receivedMessage.To = messageEventArgs.Message.To.ToString();
   //         receivedMessage.MessageText = messageEventArgs.Message.Body;
   //         //receivedMessage.Subject = messageEventArgs.Message.Subject;
   //         receivedMessage.ChatDialogId = messageEventArgs.Message.Thread;
   //         //receivedMessage.DateSent = messageEventArgs.Message.Timestamp.Ticks / 1000;
   //         var extraParams = XElement.Parse(messageEventArgs.Message.ExtraParameter);

			//var notificationType = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.notification_type));
			//if (notificationType != null)
			//{
			//	int intValue;
			//	if (int.TryParse(notificationType.Value, out intValue))
			//	{
			//		receivedMessage.NotificationType = (NotificationTypes)intValue;
			//	}
			//}

   //         var dateSent = extraParams.Element(ExtraParams.GetXNameFor(ExtraParamsList.date_sent));
   //         if (dateSent != null)
   //         {
   //             long longValue;

   //             if (long.TryParse(dateSent.Value, out longValue))
   //             {
   //                 receivedMessage.DateSent = longValue;
   //             }
   //         }

   //         receivedMessage.ExtraParameters = XElement.Parse(messageEventArgs.Message.ExtraParameter);

   //         var wappedMessageType = (MessageType)Enum.Parse(typeof(MessageType), messageEventArgs.Message.Type.ToString());
   //         //receivedMessage.MessageType = wappedMessageTyp;
   //         //receivedMessage.XmlMessage = messageEventArgs.Message.ToString();

   //         receivedMessage.SenderId = wappedMessageType == MessageType.Groupchat ? GetQbUserIdFromGroupJid(messageEventArgs.Message.From.ToString()) : 
   //                                                                                GetQbUserIdFromJid(messageEventArgs.Message.From.ToString());

   //         await LoggerHolder.Log(LogLevel.Debug, "XMPP: OnMessageReceived ====> " +
   //                         " From: " + receivedMessage.SenderId +
   //                         " To: " + receivedMessage.RecipientId +
   //                         " Body: " + receivedMessage.MessageText +
   //                         " DateSent " + receivedMessage.DateSent +
   //                         " FullXmlMessage: " + messageEventArgs.Message.DataString);

   //         var handler = MessageReceived;
   //         if (handler != null)
   //         {
   //             handler.Invoke(this, new MessageEventArgs(new Jid(messageEventArgs.Jid.ToString()), receivedMessage, wappedMessageType));
   //         }
   //     }

        #endregion

        #region Public methods

        public void SendMessage(int userId, string body, string extraParams, string dialogId, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var messageId = MongoObjectIdGenerator.GetNewObjectIdString();
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var jidString = BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            var jid = new Sharp.Xmpp.Jid(jidString);
            var message = xmppClient.SendMessage(jid, messageId, body, extraParams, subject, dialogId, wrappedMessageType);

            LoggerHolder.Log(LogLevel.Debug, "XMPP: SentMessage ====> " + message.DataString);
        }

        public void SendMessage(string jid, string body, string extraParams, string dialogId, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var messageId = MongoObjectIdGenerator.GetNewObjectIdString();
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var message = xmppClient.SendMessage(jid, messageId, body, extraParams, subject, dialogId, wrappedMessageType);

            LoggerHolder.Log(LogLevel.Debug, "XMPP: SentMessage ====> " + message.DataString);
        }

        public void ReloadContacts()
        {
            this.Contacts.Clear();
            var roster = xmppClient.GetRoster();
            foreach (var item in roster)
            {
                var subscriptionWrappedState = (SubscriptionState)Enum.Parse(typeof(SubscriptionState), item.SubscriptionState.ToString());
                RosterItem wrapper = new RosterItem(item.Jid.Node, item.Name, subscriptionWrappedState, item.Pending, item.Groups);
                this.Contacts.Add(wrapper);
            }
        }

        public void SetChatState(string otherUserJid, ChatState chatState)
        {
            var wrappedChatState = (Sharp.Xmpp.Extensions.ChatState)Enum.Parse(typeof(Sharp.Xmpp.Extensions.ChatState), chatState.ToString());
            xmppClient.SetChatState(new Sharp.Xmpp.Jid(otherUserJid), wrappedChatState);
        }

        public void SetSubscribtionStatus(string otherUserJid, SubscriptionMessageType chatState)
        {
            switch (chatState)
            {
                case SubscriptionMessageType.RequestSubscription:
                    xmppClient.RequestSubscription(new Sharp.Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.ApproveSubscription:
                    xmppClient.ApproveSubscriptionRequest(new Sharp.Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.RefuseSubscription:
                    xmppClient.RefuseSubscriptionRequest(new Sharp.Xmpp.Jid(otherUserJid));
                    break;
                case SubscriptionMessageType.RevokeSubscription:
                    xmppClient.RevokeSubscription(new Sharp.Xmpp.Jid(otherUserJid));
                    break;
                default:
                    break;
            }
        }

        public void SetActivity(GeneralActivity activity, String description)
        {
            var activityWrappedState = (Sharp.Xmpp.Extensions.GeneralActivity)Enum.Parse(typeof(Sharp.Xmpp.Extensions.GeneralActivity), activity.ToString());
            xmppClient.SetActivity(activityWrappedState, description: description);
        }

        public void SetvCardAvatar(Stream stream)
        {
            xmppClient.SetvCardAvatar(stream);
        }

        public Task<string> GetvCardAvatar(RosterItem user)
        {
            TaskCompletionSource<string> getBase64Avatar = new TaskCompletionSource<string>();

            try
            {
                xmppClient.GetvCardAvatar(new Sharp.Xmpp.Jid(user.Jid.ToString()), (base64) =>
                {
                    getBase64Avatar.SetResult(base64);
                });
            }
            catch
            {
                getBase64Avatar.SetResult(null);
            }

            return getBase64Avatar.Task;
        }

        public void Unblock(int userId)
        {
            xmppClient.Unblock(new Sharp.Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public IEnumerable<Jid> GetBlocklist()
        {
            return xmppClient.GetBlocklist().Select(baseJid => new Jid(baseJid.ToString()));
        }

        public void Buzz(int userId)
        {
            xmppClient.Buzz(new Sharp.Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public void BlockUser(int userId)
        {
            xmppClient.Block(new Sharp.Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }


        public void AddContact(RosterItem user)
        {
			xmppClient.AddContact(new Sharp.Xmpp.Jid(user.Jid.ToString()), user.Name, user.Groups != null ? user.Groups.ToArray() : null);
        }

        public void RemoveContact(int userId)
        {
            xmppClient.RemoveContact(new Sharp.Xmpp.Jid(BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint)));
        }

        public void JoinToGroup(string groupJid, string nickName)
        {
            string fullJid = string.Format("{0}/{1}", groupJid, nickName);
            xmppClient.JoinToGroup(new Sharp.Xmpp.Jid(fullJid), new Sharp.Xmpp.Jid(quickbloxClient.ChatXmppClient.MyJid.ToString()));
        }

		public void LeaveGroup(string groupJid, string nickName)
		{
			string fullJid = string.Format("{0}/{1}", groupJid, nickName);
			xmppClient.LeaveGroup(new Sharp.Xmpp.Jid(fullJid), new Sharp.Xmpp.Jid(quickbloxClient.ChatXmppClient.MyJid.ToString()));
		}

        #endregion

        #region Completed

        public async Task ConnectAsync(int userId, string password)
        {
            var timeout = new TimeSpan(0, 0, 60);
            var tcs = new TaskCompletionSource<object>();
            Contacts = new List<RosterItem>();
            Presences = new List<Jid>();

            xmppClient = new XmppClient(quickbloxClient.ChatEndpoint, MySmallJid.ToString(), password);
            xmppClient.StatusChanged += (sender, args) =>
            {
                if (tcs.Task.Status == TaskStatus.WaitingForActivation)
                    tcs.TrySetResult(null);
            };

            this.userId = userId;

            xmppClient.Tls = false;
            xmppClient.Port = 5222;
            xmppClient.Password = password;

            SubscribeEvents(xmppClient);
            xmppClient.Connect();

            var timer = new Timer(state =>
            {
                var myTcs = (TaskCompletionSource<object>)state;
                if (myTcs.Task.Status == TaskStatus.WaitingForActivation)
                    myTcs.TrySetException(new QuickbloxSdkException("Failed to fully initialize xmpp connection because of timeout."));
            },
            tcs, timeout, new TimeSpan(0, 0, 0, 0, -1));

            await tcs.Task;
        }

        public void Connect(int userId, string password)
        {
            Contacts = new List<RosterItem>();
            Presences = new List<Jid>();
            this.userId = userId;

            xmppClient = new XmppClient(quickbloxClient.ChatEndpoint, MySmallJid.ToString(), password);
            xmppClient.Tls = false;
            xmppClient.Port = 5222;
            xmppClient.Password = password;

            SubscribeEvents(xmppClient);
            xmppClient.Connect();
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
