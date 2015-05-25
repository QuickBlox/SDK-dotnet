// StartTLSState.cs
//
//Copyright � 2006 - 2012 Dieter Lunn
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

using XMPP.common;
using XMPP.tags;

namespace XMPP.states
{
	public class StartTLSState : IState
	{
        public StartTLSState(Manager manager) : base(manager) {}

		public override void Execute(Tag data = null)
		{
			if (data != null && (string)data.Name.LocalName != "proceed") return;
			Manager.Connection.EnableSSL();
			Manager.State = new ConnectedState(Manager);
			Manager.State.Execute();
		}
	}
}