using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System;
using Xmpp;
using Xmpp.Im;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    /// <summary>
    /// Provide infromation about system Message
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="messageEventArgs">The <see cref="SystemMessageEventArgs"/> instance containing the event data.</param>
    public delegate void SystemMessageEventHandler(object sender, SystemMessageEventArgs messageEventArgs);

    /// <summary>
	/// Provides data for the Message event.
	/// </summary>
	public class SystemMessageEventArgs : EventArgs
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
        public SystemMessage Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the Message.
        /// </summary>
        public MessageType MessageType
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The jid parameter or the Message
        /// parameter is null.</exception>
        public SystemMessageEventArgs(Jid jid, SystemMessage message, MessageType messageType)
        {
            Jid = jid;
            Message = message;
            MessageType = messageType;
        }
    }
}
