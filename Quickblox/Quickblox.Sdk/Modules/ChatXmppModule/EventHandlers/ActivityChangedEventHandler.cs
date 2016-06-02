using System;
using Xmpp;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public delegate void ActivityChangedEventHandler(object sender, ActivityChangedEventArgs activityChangedEventArgs);

    public class ActivityChangedEventArgs : EventArgs
    {
        public ActivityChangedEventArgs(Jid jid, String activity, String specific, String description)
        {
            Jid = jid;
            Activity = activity;
            Specific = specific;
            Description = description;
        }

        public Jid Jid
        {
            get;
            private set;
        }

        /// <summary>
        /// The general activity of the XMPP entity.
        /// </summary>
        public String Activity
        {
            get;
            private set;
        }

        /// <summary>
        /// The specific activity of the XMPP entity.
        /// </summary>
        public String Specific
        {
            get;
            private set;
        }

        /// <summary>
		/// a natural-language description of, or reason for, the activity. This
		/// may be null.
		/// </summary>
		public string Description
        {
            get;
            private set;
        }
    }
}
