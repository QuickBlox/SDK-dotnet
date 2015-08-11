using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
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
