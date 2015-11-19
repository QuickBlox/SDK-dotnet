using System;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public delegate void SubscriptionsEventHandler(object sender, SubscriptionsEventArgs subscriptionsEventArgs);

    public class SubscriptionsEventArgs : EventArgs
    {
        public SubscriptionsEventArgs(Jid jid, PresenceType presenceType)
        {
            PresenceType = presenceType;
            Jid = jid;
        }

        public Jid Jid { get; private set; }

        public PresenceType PresenceType { get; private set; }

    }
}
