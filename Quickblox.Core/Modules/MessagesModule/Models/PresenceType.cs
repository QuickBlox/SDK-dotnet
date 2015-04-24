using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public enum PresenceType
    {
        available = -1,
        subscribe = 0,
        subscribed = 1,
        unsubscribe = 2,
        unsubscribed = 3,
        unavailable = 4,
        invisible = 5,
        error = 6,
        probe = 7,
    }
}
