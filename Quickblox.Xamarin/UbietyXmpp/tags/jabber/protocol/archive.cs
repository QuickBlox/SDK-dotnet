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

using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.jabber.protocol.archive
{
    public class Namespace
    {
        public const string Name = "urn:xmpp:mam:tmp";

        public static XName query = XName.Get("query", Name);
        public static XName with = XName.Get("with", Name);
        public static XName start = XName.Get("start", Name);
        public static XName end = XName.Get("end", Name);
        public static XName result = XName.Get("result", Name);
        public static XName prefs = XName.Get("prefs", Name);
        public static XName always = XName.Get("always", Name);
        public static XName never = XName.Get("never", Name);
        public static XName jid = XName.Get("jid", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) { }
        public query(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(with))]
    public class with : Tag
    {
        public with() : base(Namespace.with) { }
        public with(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(start))]
    public class start : Tag
    {
        public start() : base(Namespace.start) { }
        public start(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(end))]
    public class end : Tag
    {
        public end() : base(Namespace.end) { }
        public end(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(result))]
    public class result : Tag
    {
        public result() : base(Namespace.result) { }
        public result(XElement other) : base(other) { }

        public string queryid { get { return (string)GetAttributeValue("queryid"); } set { SetAttributeValue("queryid", value); } }
        public string id { get { return (string)GetAttributeValue("id"); } set { SetAttributeValue("id", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(prefs))]
    public class prefs : Tag
    {
        public prefs() : base(Namespace.prefs) { }
        public prefs(XElement other) : base(other) { }

        public enum defaultEnum
        {
            always,
            never,
            roster
        }

        public defaultEnum @default { get { return GetAttributeEnum<defaultEnum>("default"); } set { SetAttributeEnum<defaultEnum>("default", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(always))]
    public class always : Tag
    {
        public always() : base(Namespace.always) { }
        public always(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(never))]
    public class never : Tag
    {
        public never() : base(Namespace.never) { }
        public never(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(jid))]
    public class jid : Tag
    {
        public jid() : base(Namespace.jid) { }
        public jid(XElement other) : base(other) { }
    }
}
