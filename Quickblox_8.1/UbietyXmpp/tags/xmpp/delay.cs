// delay.cs
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

namespace XMPP.tags.xmpp.delay
{
    public class Namespace
    {
        public static string Name = "urn:xmpp:delay";
        public static XName delay = XName.Get("delay", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(delay))]
    public class delay : Tag
    {
        public delay() : base(Namespace.delay) {} 
        public delay(XElement other) : base(other) {}

		public string from { get { return (string)GetAttributeValue("from"); } set { SetAttributeValue("from", value); } }
		public string stamp { get { return (string)GetAttributeValue("stamp"); } set { SetAttributeValue("stamp", value); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:delay'
    xmlns='urn:xmpp:delay'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0203: http://www.xmpp.org/extensions/xep-0203.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='delay'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='xs:string'>
          <xs:attribute name='from' type='xs:string' use='optional'/>
          <xs:attribute name='stamp' type='xs:string' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/