using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
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

    public class DeletedId : XElement
    {
        public static XName XName = XName.Get("deleted_id", "jabber:client");

        public DeletedId() : base(XName)
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
}
