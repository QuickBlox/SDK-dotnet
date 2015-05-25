// SASLState.cs
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
using XMPP.SASL;
using XMPP.tags;

namespace XMPP.states
{
	public class SASLState : IState
	{
        public SASLState(Manager manager) : base(manager) {}

		public override void Execute(Tag data = null)
		{
#if DEBUG
            Manager.Events.LogMessage(this, LogType.Debug, "Processing next SASL step");
#endif
			var res = Manager.SASLProcessor.Step(data);
			switch (res.Name.LocalName)
			{
                case "success":
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Success, sending start stream again");
#endif
                        Manager.IsAuthenticated = true;

                        if (Manager.Transport == Transport.Socket)
                        {
                            Manager.State = new ConnectedState(Manager);
                            Manager.State.Execute();
                        }
                        else
                        {
                            (Manager.Connection as BoSH).Restart();
                        }
                        break;
                    }
                case "failure":
                    {
                        ErrorType type = ErrorType.Undefined;

                        if (Manager.SASLProcessor is MD5Processor)
                            type = ErrorType.MD5AuthError;

                        if (Manager.SASLProcessor is PlainProcessor)
                            type = ErrorType.PLAINAuthError;

                        if (Manager.SASLProcessor is SCRAMProcessor)
                            type = ErrorType.SCRAMAuthError;

                        if (Manager.SASLProcessor is XOAUTH2Processor)
                            type = ErrorType.OAUTH2AuthError;

                        var failure = data as tags.xmpp_sasl.failure;
                        var text = string.Empty;
                        if (failure.textElements.Count() > 0)
                            text = failure.textElements.First().Value;

                        Manager.Events.Error(this, type, ErrorPolicyType.Deactivate, text);

                        return;
                    }
                default:
                    {
                        Manager.Connection.Send(res);
                        break;
                    }
			}
		}
	}
}