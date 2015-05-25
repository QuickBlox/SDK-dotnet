// ampXerrors.cs
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

namespace XMPP.tags.jabber.protocol.ampXerrors
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/amp#errors";
        public static XName failed_rules = XName.Get("failed-rules", Name);
        public static XName rule = XName.Get("rule", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(failed_rules))]
    public class failed_rules : Tag
    {
        public failed_rules() : base(Namespace.failed_rules) {} 
        public failed_rules(XElement other) : base(other) {}

        public IEnumerable<rule> ruleElements { get { return Elements<rule>(Namespace.rule); } }
    }

    [XMPPTag(typeof(Namespace), typeof(rule))]
    public class rule : Tag
    {
        public rule() : base(Namespace.rule) { }
        public rule(XElement other) : base(other) { }

		public string action { get { return (string)GetAttributeValue("action"); } set { SetAttributeValue("action", value); } }
		public string condition { get { return (string)GetAttributeValue("condition"); } set { SetAttributeValue("condition", value); } }
		public string value { get { return (string)GetAttributeValue("value"); } set { SetAttributeValue("value", value); } }

    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/amp#errors'
    xmlns='http://jabber.org/protocol/amp#errors'
    elementFormDefault='qualified'>
 
  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0079: http://www.xmpp.org/extensions/xep-0079.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='failed-rules'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='rule' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='rule'>
    <xs:complexType>
      <xs:attribute name='action' use='required' type='xs:NCName'/>
      <xs:attribute name='condition' use='required' type='xs:NCName'/>
      <xs:attribute name='value' use='required' type='xs:string'/>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/