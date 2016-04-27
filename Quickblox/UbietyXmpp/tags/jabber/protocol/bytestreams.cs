// bytestreams.cs
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

namespace XMPP.tags.jabber.protocol.bytestreams
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/bytestreams";
        public static XName query = XName.Get("query", Name);
        public static XName streamhost = XName.Get("streamhost", Name);
        public static XName streamhost_used = XName.Get("streamhost-used", Name);
        public static XName udpsuccess = XName.Get("udpsuccess", Name);
        public static XName activate = XName.Get("activate", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

        public enum modeEnum
        {
            none,
            tcp,
            udp
        }

		public streamhost streamhost { get { return Element<streamhost>(Namespace.streamhost); } }
        public streamhost_used streamhost_used { get { return Element<streamhost_used>(Namespace.streamhost_used); } }
		public activate activate { get { return Element<activate>(Namespace.activate); } }		public string dstaddr { get { return (string)GetAttributeValue("dstaddr"); } set { SetAttributeValue("dstaddr", value); } }
		public string sid { get { return (string)GetAttributeValue("sid"); } set { SetAttributeValue("sid", value); } }
        public modeEnum mode { get { return GetAttributeEnum<modeEnum>("mode"); } set { SetAttributeEnum<modeEnum>("mode", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(streamhost))]
    public class streamhost : Tag
    {
        public streamhost() : base(Namespace.streamhost) {} 
        public streamhost(XElement other) : base(other) {}

		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string host { get { return (string)GetAttributeValue("host"); } set { SetAttributeValue("host", value); } }
		public string port { get { return (string)GetAttributeValue("port"); } set { SetAttributeValue("port", value); } }

    }

    [XMPPTag(typeof(Namespace), typeof(streamhost_used))]
    public class streamhost_used : Tag
    {
        public streamhost_used() : base(Namespace.streamhost_used) {} 
        public streamhost_used(XElement other) : base(other) {}

		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(udpsuccess))]
    public class udpsuccess : Tag
    {
        public udpsuccess() : base(Namespace.udpsuccess) {} 
        public udpsuccess(XElement other) : base(other) {}

		public string dstaddr { get { return (string)GetAttributeValue("dstaddr"); } set { SetAttributeValue("dstaddr", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(activate))]
    public class activate : Tag
    {
        public activate() : base(Namespace.activate) {} 
        public activate(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/bytestreams'
    xmlns='http://jabber.org/protocol/bytestreams'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0065: http://www.xmpp.org/extensions/xep-0065.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:choice>
        <xs:element ref='streamhost' minOccurs='0' maxOccurs='unbounded'/>
        <xs:element ref='streamhost-used' minOccurs='0'/>
        <xs:element name='activate' type='xs:string' minOccurs='0'/>
      </xs:choice>
      <xs:attribute name='dstaddr' type='xs:string' use='optional'/>
      <xs:attribute name='mode' use='optional' default='tcp'>
        <xs:simpleType>
          <xs:restriction base='xs:NCName'>
            <xs:enumeration value='tcp'/>
            <xs:enumeration value='udp'/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
      <xs:attribute name='sid' type='xs:string' use='required'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='streamhost'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='jid' type='xs:string' use='required'/>
          <xs:attribute name='host' type='xs:string' use='required'/>
          <xs:attribute name='port' type='xs:string' use='optional' default='1080'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='streamhost-used'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='jid' type='xs:string' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='udpsuccess'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='dstaddr' type='xs:string' use='required'/>
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