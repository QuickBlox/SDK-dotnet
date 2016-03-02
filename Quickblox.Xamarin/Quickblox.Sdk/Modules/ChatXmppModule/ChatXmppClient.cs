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

        public event ErrorsEventHandler ErrorReceived;

        public event MessageEventHandler MessageReceived;

        public event ActivityChangedEventHandler ActivityChanged;

        public event ChatStateChangedEventHandler ChatStateChanged;

        public event MoodChangedEventHandler MoodStateChanged;

        public event RosterUpdatedEventHandler RosterUpdated;

        public event StatusEventHandler StatusChanged;
        
        public event SubscriptionsEventHandler SubscriptionsChanged;

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

        private void OnTuneCallback (object sender, Sharp.Xmpp.Extensions.TuneEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnTune:");
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

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnUnsubscribedCallback (object sender, Sharp.Xmpp.Im.UnsubscribedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnUnsubscribed:");
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Unsubscribed));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnSubscriptionRefusedCallback (object sender, Sharp.Xmpp.Im.SubscriptionRefusedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnSubscriptionRefused:");
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Refused));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnSubscriptionApprovedCallback (object sender, Sharp.Xmpp.Im.SubscriptionApprovedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnSubscriptionApproved:");
            var handler = SubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Subscribed));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnStatusChangedCallback (object sender, Sharp.Xmpp.Im.StatusEventArgs e)
		{
            if (MyJid == null)
                return;

            Debug.WriteLine("XMPP: ====> OnStatusChanged:");
            Debug.WriteLine("XMPP: ====> Jid: " + e.Jid);
            Debug.WriteLine("XMPP: ====> Status: " + e.Status.Availability);

            if (e.Jid != new Sharp.Xmpp.Jid(MyJid.ToString()))
            {
                var jid = e.Jid.ToString();
                if (e.Status.Availability == Sharp.Xmpp.Im.Availability.Online)
                {
                    if (!this.Presences.Contains(jid))
                    {
                        Debug.WriteLine("XMPP: ====> Added to Presences. Jid: " + e.Jid);
                        this.Presences.Add(jid);
                    }
                }
                else
                {
                    Debug.WriteLine("XMPP: ====> Removed to Presences. Jid: " + e.Jid);
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

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnRosterUpdatedCallback (object sender, Sharp.Xmpp.Im.RosterUpdatedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnRosterUpdated:");
            Debug.WriteLine("XMPP: ====> IsRemoved: " + e.Removed);
            Debug.WriteLine("XMPP: ====> Jid: " + e.Item.Jid + " Name: " + e.Item.Name + " SubscriptionState: " + e.Item.SubscriptionState);

            var handler = RosterUpdated;
            if (handler != null)
            {
                var subscriptionWrappedState = (SubscriptionState)Enum.Parse(typeof(SubscriptionState), e.Item.SubscriptionState.ToString());
                RosterItem wrapper = new RosterItem(e.Item.Jid.Node, e.Item.Name, subscriptionWrappedState, e.Item.Pending, e.Item.Groups);
                handler.Invoke(this, new RosterUpdatedEventArgs(wrapper, e.Removed));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnMoodChangedCallback (object sender, Sharp.Xmpp.Extensions.MoodChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnMoodChanged:");

            Debug.WriteLine("XMPP: ====> Description: " + e.Description);
            Debug.WriteLine("XMPP: ====> Jid: " + e.Jid + " Mood state: " + e.Mood);

            var handler = MoodStateChanged;
            if (handler != null)
            {
                var moodeState = (Mood)Enum.Parse(typeof(Mood), e.Mood.ToString());
                handler.Invoke(this, new MoodChangedEventArgs(new Jid(e.Jid.ToString()), moodeState, e.Description));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnChatStateChangedCallback (object sender, Sharp.Xmpp.Extensions.ChatStateChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnChatStateChanged:");
            var handler = ChatStateChanged;
            if (handler != null)
            {
                var chatState = (ChatState)Enum.Parse(typeof(ChatState), e.ChatState.ToString());
                handler.Invoke(this, new ChatStateChangedEventArgs(new Jid(e.Jid.ToString()), chatState));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnActivityChangedCallback (object sender, Sharp.Xmpp.Extensions.ActivityChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnActivityChanged:");
            var handler = ActivityChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ActivityChangedEventArgs(e.Jid.Node, e.Activity.ToString(), e.Specific.ToString(), e.Description));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnClientErrorCallback(object sender, Sharp.Xmpp.Im.ErrorEventArgs args)
        {
            Debug.WriteLine("XMPP: ====>  OnClientError:");
            
            var handler = ErrorReceived;
            if (handler != null)
            {
                handler.Invoke(sender, new ErrorEventArgs(args.Exception));
            }

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
        }

        private void OnMessageReceivedCallback(object sender, Sharp.Xmpp.Im.MessageEventArgs messageEventArgs)
        {
            Debug.WriteLine("XMPP: ====> OnMessageReceived:");

            var receivedMessage = new Message();
            receivedMessage.Id = messageEventArgs.Message.Id;
            receivedMessage.From = messageEventArgs.Message.From.ToString();
            receivedMessage.To = messageEventArgs.Message.To.ToString();
            receivedMessage.MessageText = messageEventArgs.Message.Body;
            //receivedMessage.Subject = messageEventArgs.Message.Subject;
            receivedMessage.ChatDialogId = messageEventArgs.Message.Thread;
            receivedMessage.DateSent = messageEventArgs.Message.Timestamp.Ticks / 1000;
			//receivedMessage.ExtraParameter = messageEventArgs.Message.ExtraParameter;

            var wappedMessageTyp = (MessageType)Enum.Parse(typeof(MessageType), messageEventArgs.Message.Type.ToString());
            //receivedMessage.MessageType = wappedMessageTyp;
            //receivedMessage.XmlMessage = messageEventArgs.Message.ToString();

            receivedMessage.SenderId = wappedMessageTyp == MessageType.Groupchat ? GetQbUserIdFromGroupJid(messageEventArgs.Message.From.ToString()) : 
                                                                                   GetQbUserIdFromJid(messageEventArgs.Message.From.ToString());
            
            Debug.WriteLine("XMPP: ====> " + 
                            " From: " + messageEventArgs.Message.From  + 
                            " To: " + messageEventArgs.Message.To +
                            " Body: " + messageEventArgs.Message.Body +
                            " Message: " + messageEventArgs.Message.DataString);
         
            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
            
            var handler = MessageReceived;
            if (handler != null)
            {
                handler.Invoke(this, new MessageEventArgs(new Jid(messageEventArgs.Jid.ToString()), receivedMessage));
            }
        }

        #endregion

        #region Public methods

        public void SendMessage(int userId, string body, string extraParams, string dialogId, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var jidString = BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
            var jid = new Sharp.Xmpp.Jid(jidString);
            xmppClient.SendMessage(jid, body, extraParams, subject, dialogId, wrappedMessageType);
        }

        public void SendMessage(string jid, string body, string extraParams, string dialogId, string subject = null, MessageType messageType = MessageType.Chat)
        {
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            xmppClient.SendMessage(jid, body, extraParams, subject, dialogId, wrappedMessageType);
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

  //      public void Disconnect()
  //      {
  //          xmppClient.Disconnect();
  //      }

		//public void Reconnect(){
		//	xmppClient.Reconnect ();
		//}

        /// <summary>
        /// Creates a private one-to-one chat manager.
        /// </summary>
        /// <param name="otherUserId">Another user ID</param>
        /// <param name="dialogId">Dialog ID with another user</param>
        /// <returns>PrivateChatManager instance.</returns>
        public PrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId)
        {
            return new PrivateChatManager(quickbloxClient, xmppClient, otherUserId, dialogId);
        }

        /// <summary>
        /// Creates a group chat manager.
        /// </summary>
        /// <param name="groupJid">Group XMPP room JID.</param>
        /// <param name="dialogId">Group dialog ID.</param>
        /// <returns>GroupChatManager</returns>
        public GroupChatManager GetGroupChatManager(string groupJid, string dialogId)
        {
            return new GroupChatManager(quickbloxClient, xmppClient, groupJid, dialogId);
        }

        #endregion

        #region Helpers

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
