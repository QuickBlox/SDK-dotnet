// SASLProcessor.cs
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
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using XMPP.tags;
using XMPP.common;

namespace XMPP.SASL
{
	public abstract class SASLProcessor
	{
        public SASLProcessor(Manager manager)
        {
            Manager = manager;
        }

        
		protected JID Id;
		protected string Password;

        protected readonly Manager Manager;
        private readonly Dictionary<String, String> _directives = new Dictionary<String, String>();

        public abstract Tag Step(Tag tag);

		public virtual Tag Initialize()
		{
#if DEBUG
			Manager.Events.LogMessage(this, LogType.Debug, "Initializing Base Processor");
#endif

            Id = Manager.Settings.Id;
            Password = Manager.Settings.Password;

			return null;
		}
		
		public string this[string directive]
		{
			get {
                if (_directives.ContainsKey(directive))
                    return (string)_directives[directive];
                else
                    return null;
            }
			set { _directives[directive] = value; }
		}

		protected string HexString(byte[] buff)
		{
			var sb = new StringBuilder();
			foreach (byte b in buff)
			{
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}

		protected static Int64 NextInt64()
		{
            var bytes = CryptographicBuffer.GenerateRandom(sizeof(Int64)).ToArray();
			return BitConverter.ToInt64(bytes, 0);
		}
	}
}
