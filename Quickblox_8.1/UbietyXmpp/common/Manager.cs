// Manager.cs
//
//Copyright © 2006 - 2012 Dieter Lunn
//Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Threading;
using XMPP.SASL;
using XMPP.states;

namespace XMPP.common
{
	public class Manager : IDisposable
    {
        #region Properties

        private IState _state;
        public IState State
        {
            get { return _state; }
            set
            {
                if( _state != null )
                    _state.Dispose();

                _state = value;
            }
        }

        public readonly ManualResetEvent ProcessComplete = new ManualResetEvent(true);

        public readonly Settings Settings = new Settings();
        public readonly Events Events = new Events();

        public readonly IConnection Connection;
        public readonly Parser Parser;

        public bool IsConnected { get { return State.GetType() != typeof(ClosedState); } }
        public bool IsAuthenticated { get; set; }
        public bool IsCompressed { get; set; }

        public string CompressionAlgorithm { get; set; }
        public SASLProcessor SASLProcessor { get; set; }

        public Transport Transport { get; private set; }
        #endregion

        public void Dispose() { Dispose(true); }
        virtual protected void Dispose(bool managed)
        {
            Connection.Dispose();

            this.Events.OnNewTag -= OnNewTag;
            this.Events.OnError -= OnError;
            this.Events.OnConnect -= OnConnect;
            this.Events.OnDisconnect -= OnDisconnect;
        }

        public Manager(Transport transport)
        {
            Transport = transport;

            this.Connection = transport == Transport.Socket ? new Connection(this) as IConnection : new BoSH(this) as IConnection;
            this.Parser = new Parser(this);
            this.State = new ClosedState(this);

			this.Events.OnNewTag += OnNewTag;
            this.Events.OnError += OnError;
            this.Events.OnConnect += OnConnect;
            this.Events.OnDisconnect += OnDisconnect;
        }

        #region eventhandler

        public void OnConnect(object sender, EventArgs e)
        {
            // We need an XID and Password to connect to the server.
            if (String.IsNullOrEmpty(Settings.Password))
                Events.Error(this, ErrorType.MissingPassword, ErrorPolicyType.Deactivate);
            else if (String.IsNullOrEmpty(Settings.Id))
                Events.Error(this, ErrorType.MissingJID, ErrorPolicyType.Deactivate);
            else if (String.IsNullOrEmpty(Settings.Hostname))
                Events.Error(this, ErrorType.MissingHost, ErrorPolicyType.Deactivate);
            else
            {
#if DEBUG
                Events.LogMessage(this, LogType.Info, "Connecting to {0}", Connection.Hostname);
#endif

                // Set the current state to connecting and start the process.
                State = new ConnectingState(this);
                State.Execute();
            }
        }

        public void OnDisconnect(object sender, EventArgs e)
        {
            if (State.GetType() != typeof(DisconnectState))
            {
                State = new DisconnectState(this);
                State.Execute();
            }
        }

        private void OnNewTag(object sender, TagEventArgs e)
        {
            State.Execute(e.tag);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
#if DEBUG
            Events.LogMessage(this, LogType.Error, "Error from {0}: {1}", sender.GetType().ToString(), e.type.ToString());
#endif

            if( e.policy != ErrorPolicyType.Informative )
                Events.Disconnect(this);
        }

        #endregion
    }
}