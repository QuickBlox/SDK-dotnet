using System;

namespace Xmpp.Core
{
    /// <summary>
    /// Provides data for the XmppMessage event.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The XmppMessage stanza.
        /// </summary>
        public Message Stanza
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class.
        /// </summary>
        /// <param name="stanza">The XmppMessage stanza on whose behalf the event is
        /// raised.</param>
        /// <exception cref="ArgumentNullException">The stanza parameter
        /// is null.</exception>
        public MessageEventArgs(Message stanza)
        {
            stanza.ThrowIfNull("stanza");
            Stanza = stanza;
        }
    }
}