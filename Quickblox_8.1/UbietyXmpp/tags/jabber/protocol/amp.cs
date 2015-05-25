// amp.cs
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

namespace XMPP.tags.jabber.protocol.amp
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/amp";
        public static XName amp = XName.Get("amp", Name);
        public static XName invalid_rules = XName.Get("invalid-rules", Name);
        public static XName unsupported_actions = XName.Get("unsupported-actions", Name);
        public static XName unsupported_conditions = XName.Get("unsupported-conditions", Name);
        public static XName rule = XName.Get("rule", Name);
    }

   [XMPPTag(typeof(Namespace), typeof(amp))]
    public class amp : Tag
    {
        public amp() : base(Namespace.amp) {} 
        public amp(XElement other) : base(other) {}

        public IEnumerable<rule> ruleElements { get { return Elements<rule>(Namespace.rule); } }
		public string from { get { return (string)GetAttributeValue("from"); } set { SetAttributeValue("from", value); } }
		public string per_hop { get { return (string)GetAttributeValue("per_hop"); } set { SetAttributeValue("per_hop", value); } }
		public string status { get { return (string)GetAttributeValue("status"); } set { SetAttributeValue("status", value); } }
		public string to { get { return (string)GetAttributeValue("to"); } set { SetAttributeValue("to", value); } }

    }


    [XMPPTag(typeof(Namespace), typeof(invalid_rules))]
    public class invalid_rules : Tag
    {
        public invalid_rules() : base(Namespace.invalid_rules) {} 
        public invalid_rules(XElement other) : base(other) {}

        public IEnumerable<rule> ruleElements { get { return Elements<rule>(Namespace.rule); } }
    }

    [XMPPTag(typeof(Namespace), typeof(unsupported_actions))]
    public class unsupported_actions : Tag
    {
        public unsupported_actions() : base(Namespace.unsupported_actions) {} 
        public unsupported_actions(XElement other) : base(other) {}

        public IEnumerable<rule> ruleElements { get { return Elements<rule>(Namespace.rule); } }
    }

    [XMPPTag(typeof(Namespace), typeof(unsupported_conditions))]
    public class unsupported_conditions : Tag
    {
        public unsupported_conditions() : base(Namespace.unsupported_conditions) {} 
        public unsupported_conditions(XElement other) : base(other) {}

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
    targetNamespace='http://jabber.org/protocol/amp'
    xmlns='http://jabber.org/protocol/amp'
    elementFormDefault='qualified'>
 
  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0079: http://www.xmpp.org/extensions/xep-0079.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='amp'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='rule' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='from' use='optional' type='xs:string'/>
      <xs:attribute name='per-hop' use='optional' type='xs:boolean' default='false'/>
      <xs:attribute name='status' use='optional' type='xs:NCName'/>
      <xs:attribute name='to' use='optional' type='xs:string'/>
    </xs:complexType>
  </xs:element>
 
  <xs:element name='invalid-rules'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='rule' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='unsupported-actions'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='rule' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='unsupported-conditions'>
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