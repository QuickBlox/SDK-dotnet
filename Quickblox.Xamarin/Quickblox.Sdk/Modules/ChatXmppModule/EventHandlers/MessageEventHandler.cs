﻿using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public delegate void MessageEventHandler(object sender, MessageEventArgs messageEventArgs);

    /// <summary>
	/// Provides data for the Message event.
	/// </summary>
	public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The JID of the user or resource who sent the message.
        /// </summary>
        public Jid Jid
        {
            get;
            private set;
        }

        /// <summary>
        /// The received chat message.
        /// </summary>
        public Message Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The jid parameter or the message
        /// parameter is null.</exception>
        public MessageEventArgs(Jid jid, Message message)
        {
            Jid = jid;
            Message = message;
        }
    }
}
