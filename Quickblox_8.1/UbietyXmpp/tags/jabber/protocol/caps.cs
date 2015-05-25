// caps.cs
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

namespace XMPP.tags.jabber.protocol.caps
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/caps";
        public static XName c = XName.Get("c", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(c))]
    public class c : Tag
    {
        public c() : base(Namespace.c) {} 
        public c(XElement other) : base(other) {}

		public string ext { get { return (string)GetAttributeValue("ext"); } set { SetAttributeValue("ext", value); } }
		public string hash { get { return (string)GetAttributeValue("hash"); } set { SetAttributeValue("hash", value); } }
		public string node { get { return (string)GetAttributeValue("node"); } set { SetAttributeValue("node", value); } }
		public string ver { get { return (string)GetAttributeValue("ver"); } set { SetAttributeValue("ver", value); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/caps'
    xmlns='http://jabber.org/protocol/caps'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0115: http://www.xmpp.org/extensions/xep-0115.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='c'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='ext' type='xs:NMTOKENS' use='optional'/>
          <xs:attribute name='hash' type='xs:NMTOKEN' use='required'/>
          <xs:attribute name='node' type='xs:string' use='required'/>
          <xs:attribute name='ver' type='xs:string' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/