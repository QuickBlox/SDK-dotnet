using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    /// <summary>
    /// System messages (type = headline) that are not to be saved to history
    /// </summary>
    public abstract class SystemMessage
    {
        public const string SystemMessageModuleIdentifier = "SystemNotifications";

        public DateTime DateSent { get; set; }

        public NotificationTypes NotificationType { get; set; }
    }

    public enum DialogUpdateInfos
    {
        UpdatedDialogPhoto = 1,
        UpdatedDialogName = 2,
        ModifiedOccupantsList = 3
    }

    /// <summary>
    /// GroupInfo message is sent when a user was added to a new or already existing group dialog.
    /// </summary>
    public class GroupInfoMessage : SystemMessage
    {
        public string DialogId { get; set; }

        public string RoomName { get; set; }

        public string RoomPhoto { get; set; }

        public DateTime RoomUpdatedDate { get; set; }

        public DateTime RoomLastMessageDate { get; set; }

        public string RoomJid { get; set; }

        public int[] AddedOccupantIds { get; set; }

        public DialogType DialogType { get; set; }
    }


}
