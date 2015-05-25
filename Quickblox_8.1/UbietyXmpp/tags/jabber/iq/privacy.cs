// privacy.cs
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

namespace XMPP.tags.jabber.iq.privacy
{
    public class Namespace
    {
        public static string Name = "jabber:iq:privacy";
        public static XName query = XName.Get("query", Name);
        public static XName active = XName.Get("active", Name);
        public static XName default_ = XName.Get("default", Name);
        public static XName list = XName.Get("list", Name);
        public static XName item = XName.Get("item", Name);

        public static XName iq = XName.Get("iq", Name);
        public static XName message = XName.Get("message", Name);
        public static XName presence_in = XName.Get("presence-in", Name);
        public static XName presence_out = XName.Get("presence-out", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

        public IEnumerable<active> activeElements { get { return Elements<active>(Namespace.active); } }
        public IEnumerable<default_> defaultElements { get { return Elements<default_>(Namespace.default_); } }
        public IEnumerable<list> listElements { get { return Elements<list>(Namespace.list); } }
    }

    [XMPPTag(typeof(Namespace), typeof(active))]
    public class active : Tag
    {
        public active() : base(Namespace.active) {} 
        public active(XElement other) : base(other) {}

		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }

    }


    [XMPPTag(typeof(Namespace), typeof(default_))]
    public class default_ : Tag
    {
        public default_() : base(Namespace.default_) {} 
        public default_(XElement other) : base(other) {}

		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }

    }

    [XMPPTag(typeof(Namespace), typeof(list))]
    public class list : Tag
    {
        public list() : base(Namespace.list) {} 
        public list(XElement other) : base(other) {}

		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }

        public IEnumerable<item> itemElements { get { return Elements<item>(Namespace.item); } }
    }

    [XMPPTag(typeof(Namespace), typeof(item))]
    public class item : Tag
    {
        public item() : base(Namespace.item) {} 
        public item(XElement other) : base(other) {}

        public enum actionEnum
        {
            none,
            allow,
            deny
        }

        public enum typeEnum
        {
            none,
            group,
            jid,
            subscription
        }
        public actionEnum seconds { get { return GetAttributeEnum<actionEnum>("action"); } set { SetAttributeEnum<actionEnum>("action", value); } }
		public string order { get { return (string)GetAttributeValue("order"); } set { SetAttributeValue("order", value); } }
        public typeEnum type { get { return GetAttributeEnum<typeEnum>("type"); } set { SetAttributeEnum<typeEnum>("type", value); } }
		public string value { get { return (string)GetAttributeValue("value"); } set { SetAttributeValue("value", value); } }


        public IEnumerable<iq> iqElements { get { return Elements<iq>(Namespace.iq); } }
        public IEnumerable<message> messageElements { get { return Elements<message>(Namespace.message); } }
        public IEnumerable<presence_in> presence_inElements { get { return Elements<presence_in>(Namespace.presence_in); } }
        public IEnumerable<presence_out> presence_outElements { get { return Elements<presence_out>(Namespace.presence_out); } }
    }


    [XMPPTag(typeof(Namespace), typeof(iq))]
    public class iq : Tag 
    {
        public iq() : base(Namespace.iq) {} 
        public iq(XElement other) : base(other) {} 
    }

    
    [XMPPTag(typeof(Namespace), typeof(message))]
    public class message : Tag 
    { 
        public message() : base(Namespace.message) {} 
        public message(XElement other) : base(other) {}
    }


    [XMPPTag(typeof(Namespace), typeof(presence_in))]
    public class presence_in : Tag
    { 
        public presence_in() : base(Namespace.presence_in) {} 
        public presence_in(XElement other) : base(other) {}
    }


    [XMPPTag(typeof(Namespace), typeof(presence_out))]
    public class presence_out : Tag
    { 
        public presence_out() : base(Namespace.presence_out) {} 
        public presence_out(XElement other) : base(other) {} 
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
  xmlns:xs='http://www.w3.org/2001/XMLSchema'
  targetNamespace='jabber:iq:privacy'
  xmlns='jabber:iq:privacy'
  elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0016: http://www.xmpp.org/extensions/xep-0016.html
    </xs:documentation>
  </xs:annotation>

<xs:element name='query'>
  <xs:complexType>
    <xs:sequence>
      <xs:element ref='active'
                  minOccurs='0'/>
      <xs:element ref='default'
                  minOccurs='0'/>
      <xs:element ref='list'
                  minOccurs='0'
                  maxOccurs='unbounded'/>
    </xs:sequence>
  </xs:complexType>
</xs:element>

<xs:element name='active'>
  <xs:complexType>
    <xs:simpleContent>
      <xs:extension base='xs:NMTOKEN'>
        <xs:attribute name='name'
                      type='xs:string'
                      use='optional'/>
      </xs:extension>
    </xs:simpleContent>
  </xs:complexType>
</xs:element>

<xs:element name='default'>
  <xs:complexType>
    <xs:simpleContent>
      <xs:extension base='xs:NMTOKEN'>
        <xs:attribute name='name'
                      type='xs:string'
                      use='optional'/>
      </xs:extension>
    </xs:simpleContent>
  </xs:complexType>
</xs:element>

<xs:element name='list'>
  <xs:complexType>
    <xs:sequence>
      <xs:element ref='item'
                  minOccurs='0'
                  maxOccurs='unbounded'/>
    </xs:sequence>
    <xs:attribute name='name'
                  type='xs:string'
                  use='required'/>
  </xs:complexType>
</xs:element>

<xs:element name='item'>
  <xs:complexType>
    <xs:sequence>
      <xs:element name='iq'
                  minOccurs='0'
                  type='empty'/>
      <xs:element name='message'
                  minOccurs='0'
                  type='empty'/>
      <xs:element name='presence-in'
                  minOccurs='0'
                  type='empty'/>
      <xs:element name='presence-out'
                  minOccurs='0'
                  type='empty'/>
    </xs:sequence>
    <xs:attribute name='action' use='required'>
      <xs:simpleType>
        <xs:restriction base='xs:NCName'>
          <xs:enumeration value='allow'/>
          <xs:enumeration value='deny'/>
        </xs:restriction>
      </xs:simpleType>
    </xs:attribute>
    <xs:attribute name='order'
                  type='xs:unsignedInt'
                  use='required'/>
    <xs:attribute name='type' use='optional'>
      <xs:simpleType>
        <xs:restriction base='xs:NCName'>
          <xs:enumeration value='group'/>
          <xs:enumeration value='jid'/>
          <xs:enumeration value='subscription'/>
        </xs:restriction>
      </xs:simpleType>
    </xs:attribute>
    <xs:attribute name='value'
                  type='xs:string'
                  use='optional'/>
  </xs:complexType>
</xs:element>

<xs:simpleType name='empty'>
  <xs:restriction base='xs:string'>
    <xs:enumeration value=''/>
  </xs:restriction>
</xs:simpleType>

</xs:schema>

*/