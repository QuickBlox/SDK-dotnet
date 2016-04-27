﻿using System;

namespace Sockets.Plugin.Abstractions
{
    /// <summary>
    ///     Fired when a TcpSocketListener receives a new connection.
    /// </summary>
    public class TcpSocketListenerConnectEventArgs : EventArgs
    {
        private readonly ITcpSocketClient _socketClient;

        /// <summary>
        ///     A <code>TcpSocketClient</code> representing the newly connected client.
        /// </summary>
        public ITcpSocketClient SocketClient
        {
            get { return _socketClient; }
        }

        /// <summary>
        ///     Constructor for <code>TcpSocketListenerConnectEventArgs.</code>
        /// </summary>
        /// <param name="socketClient">A <code>TcpSocketClient</code> representing the newly connected client.</param>
        public TcpSocketListenerConnectEventArgs(ITcpSocketClient socketClient)
        {
            _socketClient = socketClient;
        }
    }
}