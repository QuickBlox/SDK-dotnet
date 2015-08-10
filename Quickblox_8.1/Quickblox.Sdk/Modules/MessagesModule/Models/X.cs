using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class X : XElement
    {
        public static XName XName = XName.Get("x", "http://jabber.org/protocol/muc");

        public X() : base(XName)
        {
        }
    }

    public class History : Tag
    {
        public static XName XName = XName.Get("history", "http://jabber.org/protocol/muc");

        public History()
            : base(XName)
        {
        }

        public string Maxstanzas { get { return (string)GetAttributeValue("maxstanzas"); } set { SetAttributeValue("maxstanzas", value); } }
    }
}
