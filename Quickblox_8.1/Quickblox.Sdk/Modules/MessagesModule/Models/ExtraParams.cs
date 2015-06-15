using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class ExtraParams : XElement
    {
        public ExtraParams() : base(XName.Get("extraParams", "jabber:client"))
        {
        }
    }

    public class SaveToHistory : XElement
    {
        public SaveToHistory() : base(XName.Get("save_to_history", "jabber:client"))
        {
        }
    }
}
