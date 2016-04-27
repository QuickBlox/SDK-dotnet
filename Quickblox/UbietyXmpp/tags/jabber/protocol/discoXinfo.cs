// discoXinfo.cs
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

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.jabber.protocol.discoXinfo
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/disco#info";
        public static XName query = XName.Get("query", Name);
        public static XName identity = XName.Get("identity", Name);
        public static XName feature = XName.Get("feature", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

		public string node { get { return (string)GetAttributeValue("node"); } set { SetAttributeValue("node", value); } }

        public IEnumerable<identity> identityElements { get { return Elements<identity>(Namespace.identity); } }
        public IEnumerable<feature> featureElements { get { return Elements<feature>(Namespace.feature); } }
    }

    [XMPPTag(typeof(Namespace), typeof(identity))]
    public class identity : Tag
    {
        public identity() : base(Namespace.identity) {} 
        public identity(XElement other) : base(other) {}

        public string category { get { return (string)GetAttributeValue("category"); } set { if (value.Length < 1) { throw new Exception("Text out of range"); } SetAttributeValue("category", value); } } // nonEmptyString
		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }
        public string type { get { return (string)GetAttributeValue("type"); } set { if (value.Length < 1) { throw new Exception("Text out of range"); } SetAttributeValue("type", value); } } // nonEmptyString
    }

    [XMPPTag(typeof(Namespace), typeof(feature))]
    public class feature : Tag
    {
        public feature() : base(Namespace.feature) { }
        public feature(XElement other) : base(other) { }

		public string var { get { return (string)GetAttributeValue("var"); } set { SetAttributeValue("var", value); } }
    }

}

/*
<?xml version='1.0' encoding='UTF-8' ?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/disco#info'
    xmlns='http://jabber.org/protocol/disco#info'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0030: http://www.xmpp.org/extensions/xep-0030.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:sequence minOccurs='0'>
        <xs:element ref='identity' maxOccurs='unbounded'/>
        <xs:element ref='feature' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='node' type='xs:string' use='optional'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='identity'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='category' type='nonEmptyString' use='required'/>
          <xs:attribute name='name' type='xs:string' use='optional'/>
          <xs:attribute name='type' type='nonEmptyString' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='feature'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='var' type='xs:string' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='nonEmptyString'>
    <xs:restriction base='xs:string'>
      <xs:minLength value='1'/>
    </xs:restriction>
  </xs:simpleType>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/