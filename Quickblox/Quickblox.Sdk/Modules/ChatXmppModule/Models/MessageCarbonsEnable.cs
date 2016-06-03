using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public class MessageCarbonsEnable : XElement
    {
        public static XName XName = XName.Get("enable", "urn:xmpp:carbons:2");

        public MessageCarbonsEnable() : base(XName)
        {
        }
    }
}
