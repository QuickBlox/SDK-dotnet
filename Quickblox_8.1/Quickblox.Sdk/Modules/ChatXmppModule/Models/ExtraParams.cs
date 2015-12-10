using System.Xml.Linq;
using XMPP.tags.jabber.protocol.archive;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public enum ExtraParamsList
    {
        save_to_history,
        date_sent,
        dialog_id,
        room_photo,
        room_name,
        room_jid,
        occupants_ids,
        current_occupant_ids,
        added_occupant_ids,
        deleted_occupant_ids,
        deleted_id,
        room_updated_date,
        notification_type,
        moduleIdentifier, // for System messages
        type // dialogType (Private/Group/Public Group)
    }

    public class ExtraParams : XElement
    {
        private const string DefaultNamespace = "jabber:client";
        public static XName XName = XName.Get("extraParams", DefaultNamespace);

        public ExtraParams() : base(XName)
        {
        }

        public void AddNew(ExtraParamsList extraParam, string value)
        {
            AddNew(extraParam.ToString(), value);
        }

        public void AddNew(string paramName, string value, string paramNamespace = DefaultNamespace)
        {
            var extraParam = new XElement(XName.Get(paramName, paramNamespace)) {Value = value ?? ""};
            this.Add(extraParam);
        }

        public static XName GetXNameFor(ExtraParamsList extraParam, string paramNamespace = DefaultNamespace)
        {
            return XName.Get(extraParam.ToString(), paramNamespace);
        }
    }

}
