// xmpp_session.cs
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

using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.xmpp_session
{
    public class Namespace
    {
        public static string Name = "urn:ietf:params:xml:ns:xmpp-session";
        public static XName session = XName.Get("session", Name);
        public static XName required = XName.Get("required", Name);
        public static XName optional = XName.Get("optional", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(session))]
    public class session : Tag
    {
        public session() : base(Namespace.session) { }
        public session(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(required))]
    public class required : Tag 
    {
        public required() : base(Namespace.required) { }
        public required(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(optional))]
    public class optional : Tag 
    {
        public optional() : base(Namespace.optional) { }
        public optional(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:ietf:params:xml:ns:xmpp-session'
    xmlns='urn:ietf:params:xml:ns:xmpp-session'
    elementFormDefault='qualified'>

  <xs:element name='session' type='empty'/>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/