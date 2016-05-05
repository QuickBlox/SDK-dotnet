using System;

namespace Xmpp.Im
{
    /// <summary>
    /// Provides data for the XmppMessage event.
    /// </summary>
    internal class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The JID of the user or resource who sent the XmppMessage.
        /// </summary>
        public Jid Jid
        {
            get;
            private set;
        }

        /// <summary>
        /// The received chat XmppMessage.
        /// </summary>
        public XmppMessage xmppMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The jid parameter or the XmppMessage
        /// parameter is null.</exception>
        public MessageEventArgs(Jid jid, XmppMessage xmppXmppMessage)
        {
            jid.ThrowIfNull("jid");
            xmppXmppMessage.ThrowIfNull("message");
            Jid = jid;
            this.xmppMessage = xmppXmppMessage;
        }
    }
}