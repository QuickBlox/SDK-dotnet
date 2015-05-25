// gateway.cs
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

using System.Collections.Generic;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.jabber.iq.gateway
{
    public class Namespace
    {
        public static string Name = "jabber:iq:gateway";
        public static XName query = XName.Get("query", Name);
        public static XName desc = XName.Get("desc", Name);
        public static XName prompt = XName.Get("prompt", Name);
        public static XName jid = XName.Get("jid", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

		public jid jid { get { return Element<jid>(Namespace.jid); } }
        public IEnumerable<desc> descElements { get { return Elements<desc>(Namespace.desc); } }
        public IEnumerable<prompt> promptElements { get { return Elements<prompt>(Namespace.prompt); } }
    }


    [XMPPTag(typeof(Namespace), typeof(desc))]
    public class desc : Tag
    {
        public desc() : base(Namespace.desc) {} 
        public desc(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(prompt))]
    public class prompt : Tag
    {
        public prompt() : base(Namespace.prompt) {} 
        public prompt(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(jid))]
    public class jid : Tag
    {
        public jid() : base(Namespace.jid) {} 
        public jid(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:gateway'
    xmlns='jabber:iq:gateway'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0100: http://www.xmpp.org/extensions/xep-0100.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:choice>
        <xs:sequence>
          <xs:element name='desc' minOccurs='0' type='xs:string'/>
          <xs:element name='prompt' type='xs:string'/>
        </xs:sequence>
        <xs:element name='jid' type='xs:string'/>
      </xs:choice>
    </xs:complexType>
  </xs:element>

</xs:schema>
*/