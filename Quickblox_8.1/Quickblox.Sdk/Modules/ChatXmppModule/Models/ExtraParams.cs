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
        occupants_ids,
        deleted_id,
        notification_type,
        moduleIdentifier
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
            var extraParam = new XElement(XName.Get(paramName, paramNamespace)) {Value = value};
            this.Add(extraParam);
        }

        public static XName GetXNameFor(ExtraParamsList extraParam, string paramNamespace = DefaultNamespace)
        {
            return XName.Get(extraParam.ToString(), paramNamespace);
        }
    }

}
