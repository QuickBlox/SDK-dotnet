using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Converters;
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
        public event EventHandler<Message> MessageReceived;

        #endregion

        #region Ctor

        internal GroupChatManager(IQuickbloxClient quickbloxClient, XMPP.Client client, string groupJid, string dialogId)
        {
            this.quickbloxClient = quickbloxClient;
            xmppClient = client;
            this.groupJid = groupJid;
            this.dialogId = dialogId;
            quickbloxClient.ChatXmppClient.MessageReceived += MessagesClientOnOnMessageReceived;

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
        /// Sends an attachemnt to the group chat.
        /// </summary>
        /// <param name="attachment">Attachment</param>
        /// <returns></returns>
        public bool SendAttachemnt(AttachmentTag attachment)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Attachment" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.Add(attachment);

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

            return NotifyAbountGroupOccupantsOnCreation(dialogInfo.OccupantsIds, dialogInfo.UpdateAt);
        }

        /// <summary>
        /// Sends notification group chat message that new occupants were added to the group.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="deletedOccupantsIds">deleted occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyAboutGroupUpdate(IList<int> addedOccupantsIds, IList<int> deletedOccupantsIds, Dialog dialogInfo)
        {
            foreach (int occupant in addedOccupantsIds)
            {
                SendGroupInfoSystemMessage(occupant, dialogInfo);
            }

            return NotifyAbountGroupOccupantsOnUpdate(dialogInfo.OccupantsIds, addedOccupantsIds, deletedOccupantsIds, dialogInfo.UpdateAt);
        }

        /// <summary>
        /// Sends notification group chat message that group chat image has been changed.
        /// </summary>
        /// <param name="groupImageUrl">New group chat image URL</param>
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyGroupImageChanged(string groupImageUrl, DateTime updatedAt)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)NotificationTypes.GroupUpdate).ToString());
            extraParams.AddNew(ExtraParamsList.room_photo, groupImageUrl);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.UpdatedDialogPhoto.ToIntString());

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
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        public bool NotifyGroupNameChanged(string groupName, DateTime updatedAt)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Notification message" };

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.room_name, groupName);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.UpdatedDialogName.ToIntString());

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
            var message = new message
            {
                to = BuildUserJid(userId),
                type = XMPP.tags.jabber.client.message.typeEnum.headline
            };

            var stringIntListConverter = new StringIntListConverter();

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.moduleIdentifier, SystemMessage.SystemMessageModuleIdentifier);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupCreate.ToIntString());
            extraParams.AddNew(ExtraParamsList.date_sent, DateTime.UtcNow.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.room_updated_date, dialogInfo.UpdateAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogInfo.Id);
            extraParams.AddNew(ExtraParamsList.room_name, dialogInfo.Name);
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, stringIntListConverter.ConvertToString(dialogInfo.OccupantsIds.ToList()));
            extraParams.AddNew(ExtraParamsList.type, ((int)dialogInfo.Type).ToString());

            if (!string.IsNullOrEmpty(dialogInfo.Photo))
            {
                extraParams.AddNew(ExtraParamsList.room_photo, dialogInfo.Photo);
            }

            message.Add(extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(message);
            return true;
        }

        private bool NotifyAbountGroupOccupantsOnCreation(IList<int> addedOccupantsIds, DateTime groupCreationDate)
        {
            var msg = CreateNewMessage();

            var body = new body { Value = "Notification message." };

            var stringIntListConverter = new StringIntListConverter();
            string addedOccupants = stringIntListConverter.ConvertToString(addedOccupantsIds);

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.added_occupant_ids, addedOccupants);
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, addedOccupants);
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.ModifiedOccupantsList.ToIntString());
            extraParams.AddNew(ExtraParamsList.room_updated_date, groupCreationDate.ToUnixEpoch().ToString());

            msg.Add(body, extraParams);

            if (!xmppClient.Connected)
            {
                xmppClient.Connect();
                return false;
            }

            xmppClient.Send(msg);
            return true;
        }

        private bool NotifyAbountGroupOccupantsOnUpdate(IList<int> currentOccupantsIds, IList<int> addedOccupantsIds, IList<int> deletedOccupantsIds, DateTime updatedAt)
        {
            var msg = CreateNewMessage();

            var body = new body {Value = "Notification message."};

            var stringIntListConverter = new StringIntListConverter();

            string currentOccupants = stringIntListConverter.ConvertToString(currentOccupantsIds);

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, currentOccupants);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.ModifiedOccupantsList.ToIntString());

            if (addedOccupantsIds != null && addedOccupantsIds.Any())
            {
                var addedOccupants = stringIntListConverter.ConvertToString(addedOccupantsIds);
                extraParams.AddNew(ExtraParamsList.added_occupant_ids, addedOccupants);
            }

            if (deletedOccupantsIds != null && deletedOccupantsIds.Any())
            {
                var deletedOccupants = stringIntListConverter.ConvertToString(deletedOccupantsIds);
                extraParams.AddNew(ExtraParamsList.deleted_occupant_ids, deletedOccupants);
            }

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

                MessageReceived?.Invoke(this, message1);
            }
        }

        private string BuildUserJid(int userId)
        {
            return $"{userId}-{quickbloxClient.ChatXmppClient.ApplicationId}@{quickbloxClient.ChatXmppClient.ChatEndpoint}";
        }

        #endregion


    }
}
