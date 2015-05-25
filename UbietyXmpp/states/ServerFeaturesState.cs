// ServerFeaturesState.cs
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
using XMPP.registries;
using XMPP.SASL;
using XMPP.tags;

namespace XMPP.states
{
	public class ServerFeaturesState : IState
	{
        public ServerFeaturesState(Manager manager) : base(manager) {}

		public override void Execute(Tag data = null)
		{
			tags.streams.features features = null;

            if (data is tags.streams.stream)
			{
                tags.streams.stream stream = data as tags.streams.stream;
                if (!stream.version.StartsWith("1."))
				{
                    Manager.Events.Error(this, ErrorType.XMPPVersionNotSupported, ErrorPolicyType.Deactivate, "Expecting stream:features from 1.x server");
					return;
				}
                features = stream.features;
			}
            else if (data is tags.streams.error)
            {
                var error = data as tags.streams.error;
                string message = string.Empty;

                if( error.HasElements)
                {
                    var text = error.Element<tags.xmpp_streams.text>(tags.xmpp_streams.Namespace.text);
                    if (text != null)
                    {
                        message = text.Value;
                    }
                    else if( error.Elements().Count() > 0 )
                    {
                        var element = error.Elements().First();
                        if( element != null )
                            message = element.Name.LocalName;
                    }
                }

                Manager.Events.Error(this, ErrorType.ServerError, ErrorPolicyType.Reconnect, message);
                return;
            }
            else if (data is tags.streams.features)
            {
                features = data as tags.streams.features;
            }

            if (features != null)
			{
                if (features.starttls != null && Manager.Settings.SSL)
				{
					Manager.State = new StartTLSState(Manager);
                    tags.xmpp_tls.starttls tls = new tags.xmpp_tls.starttls(); 
					Manager.Connection.Send(tls);
					return;
				}

				if (!Manager.IsAuthenticated)
				{
#if DEBUG
                    Manager.Events.LogMessage(this, LogType.Debug, "Creating SASL Processor");
#endif
                    if ((features.mechanisms.Types & MechanismType.XOAUTH2 & Manager.Settings.AuthenticationTypes) == MechanismType.XOAUTH2)
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Creating XOAUTH2 processor");
#endif
                        Manager.SASLProcessor = new XOAUTH2Processor(Manager);
                    }
                    else if ((features.mechanisms.Types & MechanismType.SCRAM & Manager.Settings.AuthenticationTypes) == MechanismType.SCRAM)
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Creating SCRAM-SHA-1 Processor");
#endif
                        Manager.SASLProcessor = new SCRAMProcessor(Manager);
                    }
                    else if ((features.mechanisms.Types & MechanismType.DigestMD5 & Manager.Settings.AuthenticationTypes) == MechanismType.DigestMD5)
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Creating DIGEST-MD5 Processor");
#endif
                        Manager.SASLProcessor = new MD5Processor(Manager);
                    }
                    else if ((features.mechanisms.Types & MechanismType.External & Manager.Settings.AuthenticationTypes) == MechanismType.External)
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "External Not Supported");
#endif
                    }
                    else if ((features.mechanisms.Types & MechanismType.Plain & Manager.Settings.AuthenticationTypes) == MechanismType.Plain)
                    {
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Creating PLAIN SASL processor");
#endif
                        Manager.SASLProcessor = new PlainProcessor(Manager);
                    }
                    else 
					{
                        Manager.SASLProcessor = null;

                        string supported = string.Empty;
                        foreach( var element in features.mechanisms.mechanismElements )
                        {
                            if(!string.IsNullOrEmpty(supported))
                                supported += ", ";

                            supported += element.Value;
                        }

                        Manager.Events.Error(this, ErrorType.NoSupportedAuthenticationMethod, ErrorPolicyType.Deactivate, supported);
						return;
					}
#if DEBUG
                    Manager.Events.LogMessage(this, LogType.Debug, "Sending auth with mechanism type");
#endif
                    Manager.State = new SASLState(Manager);

					Manager.Connection.Send(Manager.SASLProcessor.Initialize());

					return;
				}

				// Takes place after authentication according to XEP-0170
                if (!Manager.IsCompressed && Static.CompressionRegistry.AlgorithmsAvailable && !Manager.Settings.SSL && features.compression != null)
				{
#if DEBUG
                    Manager.Events.LogMessage(this, LogType.Info, "Starting compression");
#endif
					// Do we have a stream for any of the compressions supported by the server?
					foreach (var algorithm in
                        features.compression.methods.Where(Static.CompressionRegistry.SupportsAlgorithm))
					{
#if DEBUG
                        Manager.Events.LogMessage(this, LogType.Debug, "Using {0} for compression", algorithm);
#endif
                        var c = new tags.jabber.protocol.compress.compress();
                        var m = new tags.jabber.protocol.compress.method();

						m.Value = Manager.CompressionAlgorithm = algorithm;
						c.Add(m);
						Manager.Connection.Send(c);
						Manager.State = new CompressedState(Manager);
						return;
					}
				}
#if DEBUG
                Manager.Events.LogMessage(this, LogType.Debug, "Authenticated");
#endif
                Manager.State = new BindingState(Manager);
                Manager.State.Execute();
			}
		}
	}
}