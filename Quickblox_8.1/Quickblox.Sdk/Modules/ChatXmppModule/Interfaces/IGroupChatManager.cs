using System;
using System.Collections.Generic;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.Models;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Interfaces
{
    /// <summary>
    /// Group chat manager interface.
    /// </summary>
    public interface IGroupChatManager
    {
        /// <summary>
        /// Event when a new group message is received.
        /// </summary>
        event EventHandler<Message> OnMessageReceived;

        /// <summary>
        /// Joins XMPP chat room.
        /// This is obligatory for group chat message sending/receiving.
        /// </summary>
        /// <param name="nickName">User nickname in XMPP room.</param>
        void JoinGroup(string nickName);

        /// <summary>
        /// Sends a group chat message.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <returns>Is operation successful</returns>
        bool SendMessage(string message);

        /// <summary>
        /// Sends notification group chat message that this group was created.
        /// </summary>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        bool NotifyAboutGroupCreation(IList<int> addedOccupantsIds, Dialog dialogInfo);

        /// <summary>
        /// Sends notification group chat message that new occupants were added to the group.
        /// </summary>
        /// <param name="addedOccupantsIds">Added occupants IDs</param>
        /// <param name="dialogInfo">Dialog information</param>
        /// <returns>Is operation successful</returns>
        bool NotifyAboutGroupUpdate(IList<int> addedOccupantsIds, Dialog dialogInfo);

        /// <summary>
        /// Sends notification group chat message that group chat image has been changed.
        /// </summary>
        /// <param name="groupImageUrl">New group chat image URL</param>
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        bool NotifyGroupImageChanged(string groupImageUrl, DateTime updatedAt);

        /// <summary>
        /// Sends notification group chat message that group chat name has been changed.
        /// </summary>
        /// <param name="groupName">New group chat name</param>
        /// <param name="updatedAt">DateTime when a group was updated (from update response)</param>
        /// <returns>Is operation successful</returns>
        bool NotifyGroupNameChanged(string groupName, DateTime updatedAt);
    }
}
