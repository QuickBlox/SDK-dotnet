using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    // this is identical to presence type enum in Ubiety library
    public enum PresenceType
    {
        None,
        Error,
        Probe,
        Subscribe,
        Subscribed,
        Unavailable,
        Unsubscribe,
        Unsubscribed
    }
}
