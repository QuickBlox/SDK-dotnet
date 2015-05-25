// ping.cs
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

namespace XMPP.tags.xmpp.ping
{
    public class Namespace
    {
        public static string Name = "urn:xmpp:ping";
        public static XName ping = XName.Get("ping", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(ping))]
    public class ping : Tag
    {
        public ping() : base(Namespace.ping) {} 
        public ping(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:ping'
    xmlns='urn:xmpp:ping'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0199: http://www.xmpp.org/extensions/xep-0199.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='ping' type='empty'/>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/