using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Sharp.Xmpp.Client;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class ChatXmppClient
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;

        private string chatEndpoint;
        
        readonly Regex qbJidRegex = new Regex(@"(\d+)\-(\d+)\@.+");

        public int AppId { get; private set; }

        #endregion

        #region Events

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

		internal XmppClient XmppClient
        {
            get {
                if (xmppClient == null)
                    throw new NullReferenceException("XmppClient wasn't initialized");
                return xmppClient;
            }
        }

        public Jid MyJid { get; private set; }

        public List<RosterItem> Contacts { get; private set; }

        public List<Jid> Presences { get; private set; }

		public bool IsConnected { get { return xmppClient != null && xmppClient.Connected; } }

#if DEBUG || TEST_RELEASE
        public string DebugClientName { get; set; }
#endif

        #endregion

        #region Events

        public event EventHandler OnStatusChanged;

        public event RosterUpdatedEventHandler OnRosterUpdated;

        public event EventHandler OnChatStateChanged;

        public event EventHandler<Models.Message> OnMessageReceived;

        public event SubscriptionsEventHandler OnSubscriptionsChanged;

        public event ActivityChangedEventHandler OnActivityChanged;

        public event ErrorsEventHandler OnErrorReceived;

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

        }

        private void OnUnsubscribedCallback (object sender, Sharp.Xmpp.Im.UnsubscribedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnUnsubscribed:");
            var handler = OnSubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Unsubscribed));
            }
        }

        private void OnSubscriptionRefusedCallback (object sender, Sharp.Xmpp.Im.SubscriptionRefusedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnSubscriptionRefused:");
            var handler = OnSubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Refused));
            }
        }

        private void OnSubscriptionApprovedCallback (object sender, Sharp.Xmpp.Im.SubscriptionApprovedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnSubscriptionApproved:");
            var handler = OnSubscriptionsChanged;
            if (handler != null)
            {
                handler.Invoke(this, new SubscriptionsEventArgs(new Jid(e.Jid.ToString()), PresenceType.Subscribed));
            }
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

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");

            var handler = OnStatusChanged;
            if (handler != null)
            {
                handler.Invoke(this, null);
            }
		}

		private void OnRosterUpdatedCallback (object sender, Sharp.Xmpp.Im.RosterUpdatedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====> OnRosterUpdated:");
            Debug.WriteLine("XMPP: ====> IsRemoved: " + e.Removed);
            Debug.WriteLine("XMPP: ====> Jid: " + e.Item.Jid + " Name: " + e.Item.Name + " SubscriptionState: " + e.Item.SubscriptionState);
            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");

            var handler = OnRosterUpdated;
            if (handler != null)
            {
                var subscriptionWrappedState = (SubscriptionState)Enum.Parse(typeof(SubscriptionState), e.Item.SubscriptionState.ToString());
                RosterItem wrapper = new RosterItem(e.Item.Jid.Node, e.Item.Name, subscriptionWrappedState, e.Item.Pending, e.Item.Groups);
                handler.Invoke(this, new RosterUpdatedEventArgs(wrapper, e.Removed));
            }
        }

        private void OnMoodChangedCallback (object sender, Sharp.Xmpp.Extensions.MoodChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnMoodChanged:");

        }

        private void OnChatStateChangedCallback (object sender, Sharp.Xmpp.Extensions.ChatStateChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnChatStateChanged:");
            var handler = OnChatStateChanged;
            if (handler != null)
            {
                handler.Invoke(this, null);
            }
        }

		private void OnActivityChangedCallback (object sender, Sharp.Xmpp.Extensions.ActivityChangedEventArgs e)
		{
            Debug.WriteLine("XMPP: ====>  OnActivityChanged:");
            var handler = OnActivityChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ActivityChangedEventArgs(e.Jid.Node, e.Activity.ToString(), e.Specific.ToString(), e.Description));
            }
        }

        private void OnClientErrorCallback(object sender, Sharp.Xmpp.Im.ErrorEventArgs args)
        {
            Debug.WriteLine("XMPP: ====>  OnClientError:");
            
            var handler = OnErrorReceived;
            if (handler != null)
            {
                handler.Invoke(sender, new ErrorEventArgs(args.Exception));
            }
        }

        private void OnMessageReceivedCallback(object sender, Sharp.Xmpp.Im.MessageEventArgs messageEventArgs)
        {
            Debug.WriteLine("XMPP: ====> OnMessageReceived:");

            var receivedMessage = new Message();
            receivedMessage.From = messageEventArgs.Message.From.ToString();
            receivedMessage.To = messageEventArgs.Message.To.ToString();
            receivedMessage.Body = messageEventArgs.Message.Body;
            receivedMessage.Subject = messageEventArgs.Message.Subject;
            receivedMessage.Thread = messageEventArgs.Message.Thread;
			receivedMessage.ExtraParameter = messageEventArgs.Message.ExtraParameter;

            var wappedMessageTyp = (MessageType)Enum.Parse(typeof(MessageType), messageEventArgs.Message.Type.ToString());
            receivedMessage.MessageType = wappedMessageTyp;
            receivedMessage.XmlMessage = messageEventArgs.Message.ToString();

            Debug.WriteLine("XMPP: ====> " + 
                            " From: " + messageEventArgs.Message.From  + 
                            " To: " + messageEventArgs.Message.To +
                            " Body: " + messageEventArgs.Message.Body +
                            " Message: " + messageEventArgs.Message.DataString);
            
            // QB system message
            //var notificationType = messageEventArgs.Message.ExtraParams["notification_type"];
            //if (notificationType != null)
            //{
            //    int intValue;
            //    if (int.TryParse(notificationType, out intValue))
            //    {
            //        if (Enum.IsDefined(typeof(NotificationTypes), intValue))
            //            receivedMessage.NotificationType = (NotificationTypes)intValue;
            //    }
            //}

            Debug.WriteLine("XMPP: ====> ============///////////////////////==============");
            
            var handler = OnMessageReceived;
            if (handler != null)
            {
                handler.Invoke(this, receivedMessage);
            }
        }

        #endregion

        #region Public methods

        public void SendMessage(string toJid, string body, string extraParams, string dialogId, MessageType messageType = MessageType.Chat)
        {
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var jid = new Sharp.Xmpp.Jid(toJid.ToString());
            xmppClient.SendMessage(jid, body, extraParams, null, dialogId, wrappedMessageType);
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

        public void Unblock(RosterItem user)
        {
            xmppClient.Unblock(new Sharp.Xmpp.Jid(user.Jid.ToString()));
        }

        public IEnumerable<Jid> GetBlocklist()
        {
            return xmppClient.GetBlocklist().Select(baseJid => new Jid(baseJid.ToString()));
        }

        public void Buzz(RosterItem user)
        {
            xmppClient.Buzz(new Sharp.Xmpp.Jid(user.Jid.ToString()));
        }

        public void BlockUser(RosterItem user)
        {
            xmppClient.Block(new Sharp.Xmpp.Jid(user.Jid.ToString()));
        }


        public void AddContact(RosterItem user)
        {
			xmppClient.AddContact(new Sharp.Xmpp.Jid(user.Jid.ToString()), user.Name, user.Groups != null ? user.Groups.ToArray() : null);
        }

        public void RemoveContact(RosterItem user)
        {
            xmppClient.RemoveContact(new Sharp.Xmpp.Jid(user.Jid.ToString()));
        }

        #endregion
        
		#region Completed

		public string BuildJid(int userId)
        {
            var jid = string.Format("{0}-{1}@{2}", userId, AppId, chatEndpoint);
            return jid;
        }

        public string BuildSmallJid(int userId)
        {
            var jid = string.Format("{0}-{1}", userId, AppId);
            return jid;
        }

        private int GetUserIdFromJid(string jid)
        {
            var match = qbJidRegex.Match(jid);

            if (!match.Success || match.Groups.Count == 0) return 0;
            int userId;
            if (!int.TryParse(match.Groups[1].Value, out userId)) return 0;
            return userId;
        }

		

        public IPrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId = null)
        {
            return new PrivateChatManager(quickbloxClient, otherUserId, dialogId);
        }

        //public IGroupChatManager GetGroupChatManager(string groupJid)
        //{
        //    return new GroupChatManager(xmppClient, groupJid);
        //}

        public void Connect(string chatEndpointUrl, int userId, int applicationId, string password)
        {
            Contacts = new List<RosterItem>();
            Presences = new List<Jid>();

            chatEndpoint = chatEndpointUrl;
            AppId = applicationId;
            MyJid = BuildSmallJid(userId);

            xmppClient = new XmppClient(chatEndpoint, BuildSmallJid(userId), password);
            xmppClient.Hostname = chatEndpointUrl;
            xmppClient.Tls = false;
            xmppClient.Port = 5222;
            xmppClient.Password = password;

            SubscribeEvents(xmppClient);
            xmppClient.Connect();
        }

        public void Disconnect()
        {
            UnSubcribeEvents(xmppClient);

            Contacts.Clear();
            Presences.Clear();

            chatEndpoint = null;
            AppId = 0;
            MyJid = null;

            xmppClient.Dispose();
        }

		public void Reconnect(){
			xmppClient.Reconnect ();
		}

        #endregion
    }
}
