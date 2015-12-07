using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Interfaces;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Quickblox.Sdk.Modules.Models;
using XMPP.tags.jabber.client;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    /// <summary>
    /// Manager for group dialogs.
    /// </summary>
    public class GroupChatManager : IGroupChatManager
    {
        #region Fields

        private IQuickbloxClient quickbloxClient;
        private XMPP.Client xmppClient;
        private readonly string groupJid;
        private readonly string dialogId;

        /// <summary>
        /// Event when a new group message is received.
        /// </summary>
        public event EventHandler<Message> OnMessageReceived;

        #endregion

        #region Ctor

        internal GroupChatManager(IQuickbloxClient quickbloxClient, XMPP.Client client, string groupJid, string dialogId)
        {
            this.quickbloxClient = quickbloxClient;
            xmppClient = client;
            this.groupJid = groupJid;
            this.dialogId = dialogId;
            quickbloxClient.ChatXmppClient.OnMessageReceived += MessagesClientOnOnMessageReceived;

        }

        #endregion

        #region IGroupChatManager members

        /// <summary>
        /// Sends a group chat message.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <returns>Is operation successful</returns>
        public bool SendMessage(string message)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = message };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        /// <summary>
        /// Sends notification group chat message that this group was created.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyAboutGroupCreation(IList<int> addedOccupantsIds, Dialog dialogInfo)
        {
            foreach (int occupant in addedOccupantsIds)
            {
                SendGroupInfoSystemMessage(occupant, dialogInfo);
            }

            return NotifyAbountGroupOccupants(dialogInfo.OccupantsIds, true);
        }

        /// <summary>
        /// Sends notification group chat message that new occupants were added to the group.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyAboutGroupUpdate(IList<int> addedOccupantsIds, Dialog dialogInfo)
        {
            foreach (int occupant in addedOccupantsIds)
            {
                SendGroupInfoSystemMessage(occupant, dialogInfo);
            }

            return NotifyAbountGroupOccupants(addedOccupantsIds, false);
        }

        /// <summary>
        /// Sends notification group chat message that group chat image has been changed.
        /// </summary>
        /// <param name="groupImageUrl">New group chat image URL</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyGroupImageChanged(string groupImageUrl)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)NotificationTypes.GroupUpdate).ToString());
            extraParams.AddNew(ExtraParamsList.room_photo, groupImageUrl);

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        /// <summary>
        /// Sends notification group chat message that group chat name has been changed.
        /// </summary>
        /// <param name="groupName">New group chat name</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyGroupNameChanged(string groupName)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)NotificationTypes.GroupUpdate).ToString());
            extraParams.AddNew(ExtraParamsList.room_name, groupName);

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        /// <summary>
        /// Joins XMPP chat room.
        /// This is obligatory for group chat message sending/receiving.
        /// </summary>
        /// <param name="nickName">User nickname in XMPP room.</param>
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

        #region Private methods

        private message CreateNewMessage()
        {
            return new message
            {
                to = groupJid,
                type = message.typeEnum.groupchat,
                id = MongoObjectIdGenerator.GetNewObjectIdString()
            };
        }

        private bool SendGroupInfoSystemMessage(int userId, Dialog dialogInfo)
        {
            var userJid = BuildUserJid(userId);

            var message = new message();
            message.to = userJid;
            message.type = message.typeEnum.headline;

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.moduleIdentifier, SystemMessage.SystemMessageModuleIdentifier);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupCreate.ToIntString());
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogInfo.Id);
            extraParams.AddNew(ExtraParamsList.room_name, dialogInfo.Name);
            extraParams.AddNew(ExtraParamsList.room_jid, dialogInfo.XmppRoomJid);
            extraParams.AddNew(ExtraParamsList.added_occupant_ids, BuildUsersString(dialogInfo.OccupantsIds.ToList()));
            extraParams.AddNew(ExtraParamsList.type, ((int)DialogType.Group).ToString());

            var body = new body { Value = "Notification message" };

            message.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(message);
            return true;
        }

        private bool NotifyAbountGroupOccupants(IList<int> occupantsIds, bool isGroupCreation)
        {
            var msg = CreateNewMessage();

            var body = new body {Value = "Notification message."};

            string occupantsIdsString = BuildUsersString(occupantsIds);

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)(isGroupCreation ? NotificationTypes.GroupCreate : NotificationTypes.GroupUpdate)).ToString());
            extraParams.AddNew(ExtraParamsList.occupants_ids, occupantsIdsString);

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
                if (string.IsNullOrEmpty(message1.MessageText)) return; // for IsTyping/PausedTyping messages

                OnMessageReceived?.Invoke(this, message1);
            }
        }

        private string BuildUserJid(int userId)
        {
            return $"{userId}-{quickbloxClient.ChatXmppClient.ApplicationId}@{quickbloxClient.ChatXmppClient.ChatEndpoint}";
        }

        private string BuildUsersString(IList<int> users)
        {
            if (!users.Any()) return null;

            string occupantsIdsString = users.Aggregate("", (current, occupantsId) => current + occupantsId.ToString() + ",");
            return occupantsIdsString.Trim(',');
        }

        #endregion


    }
}
