// BindingState.cs
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

using System.Linq;
using XMPP.common;
using XMPP.tags;

namespace XMPP.states
{
	public class BindingState : IState
	{
        public BindingState(Manager manager) : base(manager) {}

		public override void Execute(Tag data = null)
		{
			if (data == null)
			{
                tags.xmpp_bind.bind bindMsg = new tags.xmpp_bind.bind();
                tags.jabber.client.iq iqMsg = new tags.jabber.client.iq { id = Tag.NextId() };
                
                if (Manager.Settings.Id.Resource != null)
				{
                    Tag res = new tags.xmpp_bind.resource() as Tag;
                    res.Value = Manager.Settings.Id.Resource;
					bindMsg.Add(res);
				}

                iqMsg.type = tags.jabber.client.iq.typeEnum.set;
				iqMsg.Add(bindMsg);

                Manager.Connection.Send(iqMsg);
			}
			else
			{
                tags.jabber.client.iq iq = data as tags.jabber.client.iq;
                tags.xmpp_bind.bind bind = null;
				if (iq != null)
				{
                    if (iq.type == tags.jabber.client.iq.typeEnum.error)
					{
                        var e = iq.errorElements.First();
						if (e != null)
                            Manager.Events.Error(this, ErrorType.BindingToResourceFailed, ErrorPolicyType.Deactivate, e.Value);
					}
                    bind = iq.Element<tags.xmpp_bind.bind>(tags.xmpp_bind.Namespace.bind);
				}

                if (bind != null)
                {
                    tags.xmpp_bind.jid jid = bind.jid;
                    if (jid != null)
                        Manager.Settings.Id = jid.JID;
                }

#if DEBUG
                Manager.Events.LogMessage(this, LogType.Info, "Bind success, JID is now: {0}", Manager.Settings.Id);
#endif
                Manager.Events.Receive(this, data); 
                Manager.Events.ResourceBound(this, Manager.Settings.Id.ToString());

                Manager.State = new SessionState(Manager);
				Manager.State.Execute();
			}
		}
	}
}