// address.cs
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

namespace XMPP.tags.jabber.protocol.address
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/address";
        public static XName addresses = XName.Get("addresses", Name);
        public static XName address = XName.Get("address", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(addresses))]
    public class addresses : Tag
    {
        public addresses() : base(Namespace.addresses) {} 
        public addresses(XElement other) : base(other) {}

        public IEnumerable<address> addressElements { get { return Elements<address>(Namespace.address); } }
    }

    [XMPPTag(typeof(Namespace), typeof(address))]
    public class address : Tag
    {
        public address() : base(Namespace.address) {} 
        public address(XElement other) : base(other) {}

        public enum typeEnum
        {
            none,
            bcc,
            cc,
            noreply,
            replyroom,
            replyto,
            to
        }

		public string delivered { get { return (string)GetAttributeValue("delivered"); } set { SetAttributeValue("delivered", value); } }
		public string desc { get { return (string)GetAttributeValue("desc"); } set { SetAttributeValue("desc", value); } }
		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string node { get { return (string)GetAttributeValue("node"); } set { SetAttributeValue("node", value); } }
        public typeEnum type { get { return GetAttributeEnum<typeEnum>("type"); } set { SetAttributeEnum<typeEnum>("type", value); } }
		public string uri { get { return (string)GetAttributeValue("uri"); } set { SetAttributeValue("uri", value); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/address'
    xmlns='http://jabber.org/protocol/address'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0033: http://www.xmpp.org/extensions/xep-0033.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='addresses'>
     <xs:complexType>
        <xs:sequence>
           <xs:element ref='address'
                       minOccurs='1'
                       maxOccurs='unbounded'/>
        </xs:sequence>
     </xs:complexType>
  </xs:element>

  <xs:element name='address'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='delivered' use='optional' fixed='true'/>
          <xs:attribute name='desc' use='optional' type='xs:string'/>
          <xs:attribute name='jid' use='optional' type='xs:string'/>
          <xs:attribute name='node' use='optional' type='xs:string'/>
          <xs:attribute name='type' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='bcc'/>
                <xs:enumeration value='cc'/>
                <xs:enumeration value='noreply'/>
                <xs:enumeration value='replyroom'/>
                <xs:enumeration value='replyto'/>
                <xs:enumeration value='to'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
          <xs:attribute name='uri' use='optional' type='xs:string'/>
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