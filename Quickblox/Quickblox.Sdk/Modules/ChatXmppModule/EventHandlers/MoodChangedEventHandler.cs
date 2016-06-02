using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System;
using Xmpp;
using Xmpp.Extensions;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public delegate void MoodChangedEventHandler(object sender, MoodChangedEventArgs moodChangedEventArgs);

    public class MoodChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The JID of the XMPP entity that published the mood information.
        /// </summary>
        public Jid Jid
        {
            get;
            private set;
        }

        /// <summary>
        /// The mood of the XMPP entity.
        /// </summary>
        public Mood Mood
        {
            get;
            private set;
        }

        /// <summary>
        /// a natural-language description of, or reason for, the mood. This may be
        /// null.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MoodChangedEventArgs class.
        /// </summary>
        /// <param name="jid">The JID of the XMPP entity that published the
        /// mood information.</param>
        /// <param name="mood">One of the values from the Mood enumeration.</param>
        /// <param name="description">A natural-language description of, or
        /// reason for, the mood.</param>
        /// <exception cref="ArgumentNullException">The jid parameter is
        /// null.</exception>
        public MoodChangedEventArgs(Jid jid, Mood mood, string description = null)
        {
            Jid = jid;
            Mood = mood;
            Description = description;
        }
    }
}
