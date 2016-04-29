using System;
using System.Collections.Generic;
using System.Linq;
using Sharp.Xmpp.Client;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Quickblox.Sdk.Modules.Models;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Builder;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class GroupChatManager
    {
        private string dialogId;
        private string groupJid;
        private IQuickbloxClient quickbloxClient;

        /// <summary>
        /// Event when a new group message is received.
        /// </summary>
        public event MessageEventHandler MessageReceived;

        public GroupChatManager(IQuickbloxClient quickbloxClient, string groupJid, string dialogId)
        {
            this.quickbloxClient = quickbloxClient;
            this.groupJid = groupJid;
            this.dialogId = dialogId;
            quickbloxClient.ChatXmppClient.MessageReceived += MessagesClientOnOnMessageReceived;
        }

        /// <summary>
        /// Sends a message to other user.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <returns>Is operation successful</returns>
        public void SendMessage(string message)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, message, extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        /// <summary>
        /// Sends an attachemnt to another user.
        /// </summary>
        /// <param name="attachment">Attachment</param>
        /// <returns></returns>
        public void SendAttachemnt(AttachmentTag attachment)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.Add(attachment);

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, "Attachment", extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        /// <summary>
        /// Sends notification group chat message that this group was created.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        public void NotifyAboutGroupCreation(IList<int> addedOccupantsIds, Dialog dialogInfo)
        {
            foreach (int occupant in addedOccupantsIds)
            {
                SendGroupInfoSystemMessage(occupant, dialogInfo);
            }

            NotifyAbountGroupOccupantsOnCreation(dialogInfo.OccupantsIds, dialogInfo.UpdateAt);
        }

        /// <summary>
        /// Sends notification group chat message that new occupants were added to the group.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="deletedOccupantsIds">deleted occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        public void NotifyAboutGroupUpdate(IList<int> addedOccupantsIds, IList<int> deletedOccupantsIds, Dialog dialogInfo)
        {
            foreach (int occupant in addedOccupantsIds)
            {
                SendGroupInfoSystemMessage(occupant, dialogInfo);
            }

            NotifyAbountGroupOccupantsOnUpdate(dialogInfo.OccupantsIds, addedOccupantsIds, deletedOccupantsIds, dialogInfo.UpdateAt);
        }

        /// <summary>
        /// Sends notification group chat message that group chat image has been changed.
        /// </summary>
        /// <param name="groupImageUrl">New group chat image URL</param>
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        public void NotifyGroupImageChanged(string groupImageUrl, DateTime updatedAt)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, ((int)NotificationTypes.GroupUpdate).ToString());
            extraParams.AddNew(ExtraParamsList.room_photo, groupImageUrl);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.UpdatedDialogPhoto.ToIntString());

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, "Notification message", extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        /// <summary>
        /// Sends notification group chat message that group chat name has been changed.
        /// </summary>
        /// <param name="groupName">New group chat name</param>
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        public void NotifyGroupNameChanged(string groupName, DateTime updatedAt)
        {
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.room_name, groupName);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.UpdatedDialogName.ToIntString());

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, "Notification message", extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        /// <summary>
        /// Joins XMPP chat room.
        /// This is obligatory for group chat message sending/receiving.
        /// </summary>
        /// <param name="nickName">User nickname in XMPP room.</param>
        public void JoinGroup(string nickName)
        {
            quickbloxClient.ChatXmppClient.JoinToGroup(groupJid, nickName);
        }

		public void LeaveGroup(string nickName)
		{
			quickbloxClient.ChatXmppClient.LeaveGroup(groupJid, nickName);
		}

        private void SendGroupInfoSystemMessage(int userId, Dialog dialogInfo)
        {            
            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.moduleIdentifier, SystemMessage.SystemMessageModuleIdentifier);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupCreate.ToIntString());
            extraParams.AddNew(ExtraParamsList.date_sent, DateTime.UtcNow.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.room_updated_date, dialogInfo.UpdateAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogInfo.Id);
            extraParams.AddNew(ExtraParamsList.room_name, dialogInfo.Name);
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, string.Join(",", dialogInfo.OccupantsIds));
            extraParams.AddNew(ExtraParamsList.type, ((int)dialogInfo.Type).ToString());

            if (!string.IsNullOrEmpty(dialogInfo.Photo))
            {
                extraParams.AddNew(ExtraParamsList.room_photo, dialogInfo.Photo);
            }

            var userJid = ChatXmppClient.BuildJid(userId, quickbloxClient.ApplicationId, quickbloxClient.ChatEndpoint);
			quickbloxClient.ChatXmppClient.SendMessage(userJid, "Notification message", extraParams.ToString(), dialogId, null, MessageType.Headline);
        }

        private void NotifyAbountGroupOccupantsOnCreation(IList<int> addedOccupantsIds, DateTime groupCreationDate)
        {
            if (addedOccupantsIds == null)
                throw new ArgumentNullException(nameof(addedOccupantsIds));

            string addedOccupants = string.Join(",", addedOccupantsIds);

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.added_occupant_ids, addedOccupants);
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, addedOccupants);
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.ModifiedOccupantsList.ToIntString());
            extraParams.AddNew(ExtraParamsList.room_updated_date, groupCreationDate.ToUnixEpoch().ToString());

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, "Notification message", extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        private void NotifyAbountGroupOccupantsOnUpdate(IList<int> currentOccupantsIds, IList<int> addedOccupantsIds, IList<int> deletedOccupantsIds, DateTime updatedAt)
        {
            if (currentOccupantsIds == null)
                throw new ArgumentNullException(nameof(currentOccupantsIds));

            string currentOccupants = string.Join(",", currentOccupantsIds);

            var extraParams = new ExtraParams();
            extraParams.AddNew(ExtraParamsList.save_to_history, "1");
            extraParams.AddNew(ExtraParamsList.dialog_id, dialogId);
            extraParams.AddNew(ExtraParamsList.notification_type, NotificationTypes.GroupUpdate.ToIntString());
            extraParams.AddNew(ExtraParamsList.current_occupant_ids, currentOccupants);
            extraParams.AddNew(ExtraParamsList.room_updated_date, updatedAt.ToUnixEpoch().ToString());
            extraParams.AddNew(ExtraParamsList.dialog_update_info, DialogUpdateInfos.ModifiedOccupantsList.ToIntString());

            if (addedOccupantsIds != null && addedOccupantsIds.Any())
            {
                string addedOccupants = string.Join(",", addedOccupantsIds);
                extraParams.AddNew(ExtraParamsList.added_occupant_ids, addedOccupants);
            }

            if (deletedOccupantsIds != null && deletedOccupantsIds.Any())
            {
                string deletedOccupants = string.Join(",", deletedOccupantsIds);
                extraParams.AddNew(ExtraParamsList.deleted_occupant_ids, deletedOccupants);
            }

            quickbloxClient.ChatXmppClient.SendMessage(groupJid, "Notification message", extraParams.ToString(), dialogId, null, MessageType.Groupchat);
        }

        private void MessagesClientOnOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            if (messageEventArgs.Message.From.Contains(groupJid))
            {
                //if (string.IsNullOrEmpty(message1.Message.MessageText)) return; // for IsTyping/PausedTyping messages

                var handler = MessageReceived;
                if (handler != null)
                {
                    handler.Invoke(this, messageEventArgs);
                }
            }
        }
    }
}
