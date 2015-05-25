// JID.cs
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

using Gnu.Inet.Encoding;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace XMPP
{
	/// <summary>
	/// Manages all aspects of a users identity on an XMPP network.
	/// </summary>
	public class JID : IComparable
	{
		private string _resource;
		private string _server;
		private string _user;
		private string _xid;

		public JID(string xid)
		{
            XmppId = string.IsNullOrEmpty(xid) ? "" : xid;
		}

		public JID(string user, string server, string resource)
		{
            User = string.IsNullOrEmpty(user) ? "" : user;
            Server = string.IsNullOrEmpty(server) ? "" : server;
            Resource = string.IsNullOrEmpty(resource) ? "" : resource;
		}

		#region Properties

		/// <summary>
		/// String representation of the id.
		/// </summary>
		public string XmppId
		{
			get { return _xid ?? BuildJID(); }
			set { Parse(value); }
		}

        public string Bare
        {
            get 
            {
                if (string.IsNullOrEmpty(User)) // No user? Don't return @
                    return Server;

                return User + "@" + Server;
            }
        }

		/// <summary>
		/// Username of the user.
		/// </summary>
		public string User
		{
			get { return string.IsNullOrEmpty(_user) ? string.Empty : Unescape(_user); }
			set
			{
				var tmp = Escape(value);
				_user = Stringprep.NodePrep(tmp);
			}
		}

		/// <summary>
		/// Server the user is logged into.
		/// </summary>
		public string Server
		{
			get { return _server; }
			set { _server = (value == null) ? null : Stringprep.NamePrep(value); }
		}

		/// <summary>
		/// Resource the user is communicating from.
		/// </summary>
		public string Resource
		{
			get { return _resource; }
			set { _resource = (value == null) ? null : Stringprep.ResourcePrep(value); }
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return _xid.GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return XmppId;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is string)
			{
				return XmppId.Equals(obj);
			}
			if (!(obj is JID))
			{
				return false;
			}

			return XmppId.Equals(((JID) obj).XmppId);
		}

		#region {{ Operators }}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public static bool operator ==(JID one, JID two)
		{
			if ((object) one == null)
			{
				return ((object) two == null);
			}

			return one.Equals(two);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public static bool operator ==(string one, JID two)
		{
			if ((object) two == null)
			{
				return ((object) one == null);
			}

			return two.Equals(one);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public static bool operator !=(JID one, JID two)
		{
			if ((object) one == null)
			{
				return ((object) two != null);
			}

			return !one.Equals(two);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public static bool operator !=(string one, JID two)
		{
			if ((object) two == null)
			{
				return ((object) one != null);
			}

			return !two.Equals(one);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <returns></returns>
		public static implicit operator JID(string one)
		{
			return new JID(one);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <returns></returns>
		public static implicit operator string(JID one)
		{
            if (one != null)
                return one.XmppId;
            else
                return string.Empty;
		}

		#endregion

		#region {{ Build and Parse functions }}

		/// <summary>
		/// Builds a string version of an XID from the three parts.
		/// </summary>
		/// <returns>string version of xid</returns>
		private string BuildJID()
		{
			var sb = new StringBuilder();
			if (!string.IsNullOrEmpty(_user))
			{
				sb.Append(User);
				sb.Append("@");
			}
			sb.Append(Server);
			if (!string.IsNullOrEmpty(_resource))
			{
				sb.Append("/");
				sb.Append(Resource);
			}

			_xid = sb.ToString();
			return _xid;
		}

		/// <summary>
		/// Takes a string xid and breaks it into its parts.
		/// </summary>
		private void Parse(string id)
		{
            if (!string.IsNullOrEmpty(id))
            {
                var at = id.IndexOf('@');
                var slash = id.IndexOf('/');

                if (at == -1)
                {
                    if (slash == -1)
                    {
                        Server = id;
                    }
                    else
                    {
                        Server = id.Substring(0, slash);
                        Resource = id.Substring(slash + 1);
                    }
                }
                else
                {
                    if (slash == -1)
                    {
                        User = id.Substring(0, at);
                        Server = id.Substring(at + 1);
                    }
                    else
                    {
                        if (at < slash)
                        {
                            User = id.Substring(0, at);
                            Server = id.Substring(at + 1, slash - at - 1);
                            Resource = id.Substring(slash + 1);
                        }
                        else
                        {
                            Server = id.Substring(0, slash);
                            Resource = id.Substring(slash + 1);
                        }
                    }
                }
            }
		}

		#endregion

		#region {{ XEP-0106 JID Escaping }}

		private static string Escape(string user)
		{
			var u = new StringBuilder();
			var count = 0;

			foreach (var c in user)
			{
				switch (c)
				{
					case ' ':
						if ((count == 0) || (count == (user.Length - 1)))
							throw new FormatException("Username cannot start or end with a space");
						u.Append(@"\20");
						break;
					case '"':
						u.Append(@"\22");
						break;
					case '&':
						u.Append(@"\26");
						break;
					case '\'':
						u.Append(@"\27");
						break;
					case '/':
						u.Append(@"\2f");
						break;
					case ':':
						u.Append(@"\3a");
						break;
					case '<':
						u.Append(@"\3c");
						break;
					case '>':
						u.Append(@"\3e");
						break;
					case '@':
						u.Append(@"\40");
						break;
					case '\\':
						u.Append(@"\5c");
						break;
					default:
						u.Append(c);
						break;
				}
				count++;
			}

			return u.ToString();
		}

		private string Unescape(string user)
		{
			var re = new Regex(@"\\([2-5][0267face])");
            var u = re.Replace(user, delegate(Match m)
			                             	{
			                             		switch (m.Groups[1].Value)
			                             		{
			                             			case "20":
			                             				return " ";
			                             			case "22":
			                             				return "\"";
			                             			case "26":
			                             				return "&";
			                             			case "27":
			                             				return "'";
			                             			case "2f":
			                             				return "/";
			                             			case "3a":
			                             				return ":";
			                             			case "3c":
			                             				return "<";
			                             			case "3e":
			                             				return ">";
			                             			case "40":
			                             				return "@";
			                             			case "5c":
			                             				return @"\";
			                             			default:
			                             				return m.Groups[0].Value;
			                             		}
			                             	});

			return u;
		}

		#endregion
	}
}