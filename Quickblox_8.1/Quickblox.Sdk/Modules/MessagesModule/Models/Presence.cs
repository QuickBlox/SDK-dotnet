using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class Presence
    {
        public PresenceType PresenceType { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }
}
