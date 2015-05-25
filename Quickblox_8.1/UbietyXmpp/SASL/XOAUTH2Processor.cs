// XOAUTH2Processor.cs
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
using System.Text;
using System.Xml.Linq;
using XMPP.common;
using XMPP.tags;

namespace XMPP.SASL
{
	public class XOAUTH2Processor : SASLProcessor
	{
        public XOAUTH2Processor(Manager manager) : base(manager) {}

        public override Tag Step(Tag tag)
		{
            if (tag.Name.LocalName == "success")
            {
#if DEBUG
                Manager.Events.LogMessage(this, LogType.Debug, "Plan login successful");
#endif
            }

            return tag;
		}

		public override Tag Initialize()
		{
#if DEBUG
			Manager.Events.LogMessage(this, LogType.Debug, "Initializing XOAUTH2 Processor");
			Manager.Events.LogMessage(this, LogType.Debug, "ID User: {0}", Manager.Settings.Id);
#endif

            string token = "";

            tags.xmpp_sasl.auth authtag = new tags.xmpp_sasl.auth();

            authtag.mechanism = MechanismType.XOAUTH2;

            XNamespace auth = "http://www.google.com/talk/protocol/auth";
            authtag.Add(new XAttribute(XNamespace.Xmlns + "auth", "http://www.google.com/talk/protocol/auth"));
            authtag.Add(new XAttribute(auth + "service", "oauth2"));

            var sb = new StringBuilder();

            sb.Append((char)0);
            sb.Append(Manager.Settings.Id);
            sb.Append((char)0);
            sb.Append(token);

            authtag.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));
            
            return authtag as Tag;
		}
	}
}