// search.cs
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

namespace XMPP.tags.jabber.iq.search
{
    public class Namespace
    {
        public static string Name = "jabber:iq:search";
        public static XName query = XName.Get("query", Name);
        public static XName instructions = XName.Get("instructions", Name);
        public static XName first = XName.Get("first", Name);
        public static XName last = XName.Get("last", Name);
        public static XName nick = XName.Get("nick", Name);
        public static XName email = XName.Get("email", Name);
        public static XName item = XName.Get("item", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

		public instructions instructions { get { return Element<instructions>(Namespace.instructions); } }
		public first first { get { return Element<first>(Namespace.first); } }
		public last last { get { return Element<last>(Namespace.last); } }
		public nick nick { get { return Element<nick>(Namespace.nick); } }
		public email email { get { return Element<email>(Namespace.email); } }
		public item item { get { return Element<item>(Namespace.item); } }
    }

    [XMPPTag(typeof(Namespace), typeof(instructions))]
    public class instructions : Tag
    {
        public instructions() : base(Namespace.instructions) { }
        public instructions(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(first))]
    public class first : Tag
    {
        public first() : base(Namespace.first) { }
        public first(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(last))]
    public class last : Tag
    {
        public last() : base(Namespace.last) { }
        public last(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(nick))]
    public class nick : Tag
    {
        public nick() : base(Namespace.nick) { }
        public nick(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(email))]
    public class email : Tag
    {
        public email() : base(Namespace.email) { }
        public email(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(item))] 
    public class item : Tag 
    { 
        public item() : base(Namespace.item) {} 
        public item(XElement other) : base(other) {} 

		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }

		public first first { get { return Element<first>(Namespace.first); } }
		public last last { get { return Element<last>(Namespace.last); } }
		public nick nick { get { return Element<nick>(Namespace.nick); } }
		public email email { get { return Element<email>(Namespace.email); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:search'
    xmlns='jabber:iq:search'
    elementFormDefault='qualified'>

  <xs:import namespace='jabber:x:data'
             schemaLocation='http://xmpp.org/schemas/x-data.xsd'/>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0055: http://xmpp.org/extensions/xep-0055.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:choice>
        <xs:choice xmlns:xdata='jabber:x:data'>
          <xs:element name='instructions' type='xs:string' minOccurs='0'/>
          <xs:element name='first' type='xs:string' minOccurs='0'/>
          <xs:element name='last' type='xs:string' minOccurs='0'/>
          <xs:element name='nick' type='xs:string' minOccurs='0'/>
          <xs:element name='email' type='xs:string' minOccurs='0'/>
          <xs:element ref='xdata:x' minOccurs='0'/>
        </xs:choice>
        <xs:element ref='item' minOccurs='0' maxOccurs='unbounded'/>
      </xs:choice>
    </xs:complexType>
  </xs:element>

  <xs:element name='item'>
    <xs:complexType>
      <xs:all>
        <xs:element name='first' type='xs:string'/>
        <xs:element name='last' type='xs:string'/>
        <xs:element name='nick' type='xs:string'/>
        <xs:element name='email' type='xs:string'/>
      </xs:all>
      <xs:attribute name='jid' type='xs:string' use='required'/>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/