using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.bosh
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/httpbind";
        public static string Xmpp = "urn:xmpp:xbosh";
        public static XName body = XName.Get("body", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(body))]
    public class body : Tag
    {
        public body() : base(Namespace.body) { }
        public body(XElement other) : base(other) { }

        public int? wait { get { return GetAttributeValueAsInt("wait"); } set { SetAttributeValue("wait", value); } }
        public int? inactivity { get { return GetAttributeValueAsInt("inactivity"); } set { SetAttributeValue("inactivity", value); } }
        public int? polling { get { return GetAttributeValueAsInt("polling"); } set { SetAttributeValue("polling", value); } }
        public int? requests { get { return GetAttributeValueAsInt("requests"); } set { SetAttributeValue("requests", value); } }
        public int? hold { get { return GetAttributeValueAsInt("hold"); } set { SetAttributeValue("hold", value); } }
        public string sid { get { return (string)GetAttributeValue("sid"); } set { SetAttributeValue("sid", value); } }
        public long? rid { get { return GetAttributeValueAsLong("rid"); } set { SetAttributeValue("rid", value); } }
        public string from { get { return (string)GetAttributeValue("from"); } set { SetAttributeValue("from", value); } }
        public string to { get { return (string)GetAttributeValue("to"); } set { SetAttributeValue("to", value); } }
        public string type { get { return (string)GetAttributeValue("type"); } set { SetAttributeValue("type", value); } }
        public bool? restart { get { return GetAttributeValueAsBool(XName.Get("restart", Namespace.Xmpp)); } set { SetAttributeValue(XName.Get("restart", Namespace.Xmpp), value); } }

        public string version { get { return (string)GetAttributeValue(XName.Get("version", Namespace.Xmpp)); } set { SetAttributeValue(XName.Get("version", Namespace.Xmpp), value); } }
        public string lang { get { return (string)GetAttributeValue("lang"); } set { SetAttributeValue("lang", value); } }
    }
}
