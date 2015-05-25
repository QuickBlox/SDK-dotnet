// Client.cs
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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using XMPP.common;
using XMPP.tags;

namespace XMPP
{
    public enum Transport
    {
        Socket,
        BoSH
    }

	public class Client : IDisposable
	{
        private readonly Manager Manager;

	    public Client() : this(Transport.Socket)
	    {
	    }

	    public Client(Transport transport)
        {
            Transport = transport;

            Manager = new Manager(transport);
        }

        public void Dispose() { Dispose(true); }

        virtual protected void Dispose(bool managed)
        {
            Manager.Dispose();
        }

        public Transport Transport { get; private set; }

        #region properties

        public readonly string Version = typeof(Client).GetTypeInfo().Assembly.GetName().Version.ToString();
        public Settings Settings { get { return Manager.Settings; } }
        public bool Connected { get { return Manager.IsConnected; } }

        public ManualResetEvent ProcessComplete { get { return Manager.ProcessComplete; } }

        #endregion

        #region events

        public event Events.ExternalError OnError { add { Manager.Events.OnError += value; } remove { Manager.Events.OnError -= value; } }
        public event Events.ExternalNewTag OnNewTag { add { Manager.Events.OnNewTag += value; } remove { Manager.Events.OnNewTag -= value; } }

        public event Events.ExternalLogMessage OnLogMessage { add { Manager.Events.OnLogMessage += value; } remove { Manager.Events.OnLogMessage -= value; } }

        public event Events.ExternalReady OnReady { add { Manager.Events.OnReady += value; } remove { Manager.Events.OnReady -= value; } }
        public event Events.ExternalResourceBound OnResourceBound { add { Manager.Events.OnResourceBound += value; } remove { Manager.Events.OnResourceBound -= value; } }
        public event Events.ExternalConnected OnConnected { add { Manager.Events.OnConnected += value; } remove { Manager.Events.OnConnected -= value; } }
        public event Events.ExternalDisconnected OnDisconnected { add { Manager.Events.OnDisconnected += value; } remove { Manager.Events.OnDisconnected -= value; } }

        public event Events.ExternalReceive OnReceive { add { Manager.Events.OnReceive += value; } remove { Manager.Events.OnReceive -= value; } }

        public event Events.ExternalChunk OnChunk { add { Manager.Events.OnChunk += value; } remove { Manager.Events.OnChunk -= value; } }
        #endregion

        #region actions

	    public void Connect()
	    {
	        Manager.Events.Connect(null, default(EventArgs));
	    }

        public Task ConnectAsync()
        {
            return Task.Run(() => Connect());
        }

	    public void Disconnect()
	    {
	        Manager.Events.Disconnect(null, default(EventArgs));
	    }

        public Task DisconnectAsync()
	    {
            return Task.Run(() => Disconnect());
	    }

	    public void Send(Tag tag)
	    {
	        Send(new TagEventArgs(tag));
	    }

        public Task SendAsync(Tag tag)
        {
            return Task.Run(() => Send(tag));
        }

	    public void Send(TagEventArgs e)
	    {
	        Manager.Events.Send(null, e);
	    }

        #endregion
    }
}