// metadata.cs
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

namespace XMPP.tags.xmpp.avatar.metadata
{
    public class Namespace
    {
        public static string Name = "urn:xmpp:avatar:metadata";
        public static XName metadata = XName.Get("metadata", Name);
        public static XName info = XName.Get("info", Name);
        public static XName pointer = XName.Get("pointer", Name);
        public static XName stop = XName.Get("stop", Name);

    }

    [XMPPTag(typeof(Namespace), typeof(metadata))]
    public class metadata : Tag 
    { 
        public metadata() : base(Namespace.metadata) {} 
        public metadata(XElement other) : base(other) {}

		public info info { get { return Element<info>(Namespace.info); } }
		public pointer pointer { get { return Element<pointer>(Namespace.pointer); } }
		public stop stop { get { return Element<stop>(Namespace.stop); } }
    }

    [XMPPTag(typeof(Namespace), typeof(info))]
    public class info : Tag 
    { 
        public info() : base(Namespace.info) {} 
        public info(XElement other) : base(other) {}

		public string bytes { get { return (string)GetAttributeValue("bytes"); } set { SetAttributeValue("bytes", value); } }
		public string height { get { return (string)GetAttributeValue("height"); } set { SetAttributeValue("height", value); } }
		public string id { get { return (string)GetAttributeValue("id"); } set { SetAttributeValue("id", value); } }
		public string type { get { return (string)GetAttributeValue("type"); } set { SetAttributeValue("type", value); } }
		public string url { get { return (string)GetAttributeValue("url"); } set { SetAttributeValue("url", value); } }
		public string width { get { return (string)GetAttributeValue("width"); } set { SetAttributeValue("width", value); } }
    }


    [XMPPTag(typeof(Namespace), typeof(pointer))]
    public class pointer : Tag  
    {
        public pointer() : base(Namespace.pointer) {} 
        public pointer(XElement other) : base(other) {} 
    }

    [XMPPTag(typeof(Namespace), typeof(stop))]
    public class stop : Tag 
    {
        public stop() : base(Namespace.stop) {} 
        public stop(XElement other) : base(other) {}
    }

}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:avatar:metadata'
    xmlns='urn:xmpp:avatar:metadata'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0084: http://www.xmpp.org/extensions/xep-0084.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='metadata'>
    <xs:complexType>
      <xs:choice>
        <xs:sequence minOccurs='0' maxOccurs='1'>
          <xs:element ref='info' minOccurs='1' maxOccurs='unbounded'/>
          <xs:element ref='pointer' minOccurs='0' maxOccurs='unbounded'/>
        </xs:sequence>
        <xs:element name='stop' type='empty'/>
      </xs:choice>
    </xs:complexType>
  </xs:element>

  <xs:element name='info'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='bytes' type='xs:unsignedShort' use='required'/>
          <xs:attribute name='height' type='xs:unsignedByte' use='optional'/>
          <xs:attribute name='id' type='xs:string' use='required'/>
          <xs:attribute name='type' type='xs:string' use='required'/>
          <xs:attribute name='url' type='xs:anyURI' use='optional'/>
          <xs:attribute name='width' type='xs:unsignedByte' use='optional'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='pointer'>
    <xs:complexType>
      <xs:sequence>
        <xs:any namespace='##other'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/