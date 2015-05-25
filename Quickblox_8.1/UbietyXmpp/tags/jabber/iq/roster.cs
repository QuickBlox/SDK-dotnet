// roster.cs
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

namespace XMPP.tags.jabber.iq.roster
{
    public class Namespace
    {
        public static string Name = "jabber:iq:roster";
        public static XName query = XName.Get("query", Name);
        public static XName item = XName.Get("item", Name);
        public static XName group = XName.Get("group", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

		public string ver { get { return (string)GetAttributeValue("ver"); } set { SetAttributeValue("ver", value); } }
        public IEnumerable<item>    itemElements { get { return Elements<item>(Namespace.item); } }
    }

    [XMPPTag(typeof(Namespace), typeof(item))]
    public class item : Tag
    {
        public item() : base(Namespace.item) {} 
        public item(XElement other) : base(other) {}

        public enum subscriptionEnum
        {
            none,
            both,
            from,
            remove,
            to
        }

        public enum askEnum
        {
            none,
            subscribe
        }

		public string approved { get { return (string)GetAttributeValue("approved"); } set { SetAttributeValue("approved", value); } }
        public askEnum ask { get { return GetAttributeEnum<askEnum>("ask"); } set { SetAttributeEnum<askEnum>("ask", value); } }
		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }
        public subscriptionEnum subscription { get { return GetAttributeEnum<subscriptionEnum>("subscription"); } set { SetAttributeEnum<subscriptionEnum>("subscription", value); } }

        public IEnumerable<group>    groupElements { get { return Elements<group>(Namespace.group); } }
    }


    [XMPPTag(typeof(Namespace), typeof(group))]
    public class group : Tag
    {
        public group() : base(Namespace.group) {} 
        public group(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:roster'
    xmlns='jabber:iq:roster'
    elementFormDefault='qualified'>

  <xs:element name='query'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='item'
                    minOccurs='0'
                    maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='ver'
                    type='xs:string'
                    use='optional'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='item'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='group'
                    minOccurs='0'
                    maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='approved'
                    type='xs:boolean'
                    use='optional'/>
      <xs:attribute name='ask' 
                    use='optional'>
        <xs:simpleType>
          <xs:restriction base='xs:NMTOKEN'>
            <xs:enumeration value='subscribe'/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
      <xs:attribute name='jid'
                    type='xs:string'
                    use='required'/>
      <xs:attribute name='name'
                    type='xs:string'
                    use='optional'/>
      <xs:attribute name='subscription' 
                    use='optional'
                    default='none'>
        <xs:simpleType>
          <xs:restriction base='xs:NMTOKEN'>
            <xs:enumeration value='both'/>
            <xs:enumeration value='from'/>
            <xs:enumeration value='none'/>
            <xs:enumeration value='remove'/>
            <xs:enumeration value='to'/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
    </xs:complexType>
  </xs:element>

  <xs:element name='group' type='xs:string'/>

</xs:schema>

*/