using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public class Tag : XElement
    {
        protected Tag(XName identity) : base(identity) { }
        protected Tag(XElement other) : base(other) { }

        public object GetAttributeValue(XName name)
        {
            XAttribute attr = Attribute(name);
            if (attr != null)
                return attr.Value;
            else
                return default(object);
        }
    }
}
