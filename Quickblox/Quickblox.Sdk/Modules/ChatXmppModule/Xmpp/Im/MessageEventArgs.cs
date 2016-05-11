using System;

namespace Xmpp.Im
{
    /// <summary>
    /// Provides data for the Message event.
    /// </summary>
    internal class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The JID of the user or resource who sent the Message.
        /// </summary>
        public Jid Jid
        {
            get;
            private set;
        }

        /// <summary>
        /// The received chat Message.
        /// </summary>
        public Message xmppMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The jid parameter or the Message
        /// parameter is null.</exception>
        public MessageEventArgs(Jid jid, Message xmppXmppMessage)
        {
            jid.ThrowIfNull("jid");
            xmppXmppMessage.ThrowIfNull("message");
            Jid = jid;
            this.xmppMessage = xmppXmppMessage;
        }
    }
}