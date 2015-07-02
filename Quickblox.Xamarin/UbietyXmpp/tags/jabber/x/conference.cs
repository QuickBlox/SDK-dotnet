// conference.cs
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

namespace XMPP.tags.jabber.x.conference
{
    public class Namespace
    {
        public static string Name = "jabber:x:conference";
        public static XName x = XName.Get("x", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(x))]
    public class x : Tag
    {
        public x() : base(Namespace.x) {} 
        public x(XElement other) : base(other) {}

		public string continue_ { get { return (string)GetAttributeValue("continue_"); } set { SetAttributeValue("continue_", value); } }
		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string password { get { return (string)GetAttributeValue("password"); } set { SetAttributeValue("password", value); } }
		public string reason { get { return (string)GetAttributeValue("reason"); } set { SetAttributeValue("reason", value); } }
		public string thread { get { return (string)GetAttributeValue("thread"); } set { SetAttributeValue("thread", value); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:x:conference'
    xmlns='jabber:x:conference'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0249: http://www.xmpp.org/extensions/xep-0249.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='x'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute
              name='continue'
              type='xs:boolean'
              use='optional'/>
          <xs:attribute
              name='jid'
              type='xs:string'
              use='required'/>
          <xs:attribute
              name='password'
              type='xs:string'
              use='optional'/>
          <xs:attribute
              name='reason'
              type='xs:string'
              use='optional'/>
          <xs:attribute
              name='thread'
              type='xs:string'
              use='optional'/>
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