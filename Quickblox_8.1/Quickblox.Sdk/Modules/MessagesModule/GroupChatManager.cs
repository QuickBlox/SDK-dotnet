using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using XMPP.tags.jabber.client;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    public class GroupChatManager : IGroupChatManager
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        private readonly string groupJid;
        private readonly string dialogId;

        public event EventHandler<Message> OnMessageReceived;

        #endregion

        #region Ctor

        internal GroupChatManager(IQuickbloxClient quickbloxClient, XMPP.Client client, string groupJid, string dialogId)
        {
            this.quickbloxClient = quickbloxClient;
            xmppClient = client;
            this.groupJid = groupJid;
            this.dialogId = dialogId;
            quickbloxClient.MessagesClient.OnMessageReceived += MessagesClientOnOnMessageReceived;
        }

        #endregion

        #region IGroupChatManager members

        public bool SendMessage(string message)
        {
            var msg = new message
            {
                to = groupJid,
                type = XMPP.tags.jabber.client.message.typeEnum.groupchat
            };

            var body = new body { Value = message };

            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });
            extraParams.Add(new DialogId { Value = dialogId });

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public bool NotifyAboutGroupCreation(IList<int> occupantsIds)
        {
            return NotifyAbountGroupOccupants(occupantsIds, true);
        }

        public bool NotifyAboutGroupUpdate(IList<int> addedOccupantsIds)
        {
            return NotifyAbountGroupOccupants(addedOccupantsIds, false);
        }

        public bool NotifyGroupImageChanged(string groupImageUrl)
        {
            var msg = new message
            {
                to = groupJid,
                type = XMPP.tags.jabber.client.message.typeEnum.groupchat
            };

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });
            extraParams.Add(new DialogId { Value = dialogId });
            extraParams.Add(new NotificationType { Value = ((int)NotificationTypes.GroupUpdate).ToString() });
            extraParams.Add(new RoomPhoto { Value = groupImageUrl });

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public bool NotifyGroupNameChanged(string groupName)
        {
            var msg = new message
            {
                to = groupJid,
                type = XMPP.tags.jabber.client.message.typeEnum.groupchat
            };

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });
            extraParams.Add(new DialogId { Value = dialogId });
            extraParams.Add(new NotificationType { Value = ((int)NotificationTypes.GroupUpdate).ToString() });
            extraParams.Add(new RoomName { Value = groupName });

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        public void JoinGroup(string nickName)
        {
            string fullJid = string.Format("{0}/{1}", groupJid, nickName);

            var presense = new presence {to = fullJid};
            X x = new X();
            x.Add(new History() {Maxstanzas = "0"});
            presense.Add(x);

            xmppClient.Send(presense);
        }

        #endregion

        private bool NotifyAbountGroupOccupants(IList<int> occupantsIds, bool isGroupCreation)
        {
            var msg = new message
            {
                to = groupJid,
                type = XMPP.tags.jabber.client.message.typeEnum.groupchat
            };

            var body = new body { Value = "Notification message." };

            string occupantsIdsString = occupantsIds.Aggregate("", (current, occupantsId) => current + occupantsId.ToString() + ",");
            occupantsIdsString = occupantsIdsString.Trim(',');

            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });
            extraParams.Add(new DialogId { Value = dialogId });
            extraParams.Add(new NotificationType { Value = ((int)(isGroupCreation ? NotificationTypes.GroupCreate : NotificationTypes.GroupUpdate)).ToString() });
            extraParams.Add(new OccupantsIds { Value = occupantsIdsString });

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        private void MessagesClientOnOnMessageReceived(object sender, Message message1)
        {
            if (message1.From.Contains(groupJid))
            {
                var handler = OnMessageReceived;
                if (handler != null)
                    handler(this, message1);
            }
        }

    }
}
