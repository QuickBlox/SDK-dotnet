using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class ExtraParams : XElement
    {
        public static XName XName = XName.Get("extraParams", "jabber:client");

        public ExtraParams() : base(XName)
        {
        }
    }

    public class SaveToHistory : XElement
    {
        public static XName XName = XName.Get("save_to_history", "jabber:client");

        public SaveToHistory() : base(XName)
        {
        }
    }

    public class DateSent : XElement
    {
        public static XName XName = XName.Get("date_sent", "jabber:client");

        public DateSent() : base(XName)
        {
        }
    }

    public class DialogId : XElement
    {
        public static XName XName = XName.Get("dialog_id", "jabber:client");

        public DialogId() : base(XName)
        {
        }
    }

    public class RoomPhoto : XElement
    {
        public static XName XName = XName.Get("room_photo", "jabber:client");

        public RoomPhoto() : base(XName)
        {
        }
    }

    public class RoomName : XElement
    {
        public static XName XName = XName.Get("room_name", "jabber:client");

        public RoomName()
            : base(XName)
        {
        }
    }

    public class OccupantsIds : XElement
    {
        public static XName XName = XName.Get("occupants_ids", "jabber:client");

        public OccupantsIds() : base(XName)
        {
        }
    }

    public class NotificationType : XElement
    {
        public static XName XName = XName.Get("notification_type", "jabber:client");

        public NotificationType() : base(XName)
        {
        }
    }

    public enum NotificationTypes
    {
        GroupCreate = 1,
        GroupUpdate = 2,
        FriendsRequest = 4,
        FriendsAccept = 5,
        FriendsReject = 6,
        FriendsRemove = 7,
        AddedDialog = 21,
        NameDialog = 22,
        PhotoDialog = 23,
        LeaveDialog = 24,
        CreateDialog = 25
    }
}
