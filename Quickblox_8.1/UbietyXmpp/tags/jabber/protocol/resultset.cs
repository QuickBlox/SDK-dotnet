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

namespace XMPP.tags.jabber.protocol.resultset
{
    public class Namespace
    {
        public const string Name = "http://jabber.org/protocol/rsm";

        public static XName set = XName.Get("set", Name);
        public static XName after = XName.Get("after", Name);
        public static XName before = XName.Get("before", Name);
        public static XName count = XName.Get("count", Name);
        public static XName first = XName.Get("first", Name);
        public static XName index = XName.Get("index", Name);
        public static XName last = XName.Get("last", Name);
        public static XName max = XName.Get("max", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(set))]
    public class set : Tag
    {
        public set() : base(Namespace.set) { }
        public set(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(after))]
    public class after : Tag
    {
        public after() : base(Namespace.after) { }
        public after(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(before))]
    public class before : Tag
    {
        public before() : base(Namespace.before) { }
        public before(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(count))]
    public class count : Tag
    {
        public count() : base(Namespace.count) { }
        public count(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(first))]
    public class first : Tag
    {
        public first() : base(Namespace.first) { }
        public first(XElement other) : base(other) { }

        public int? index { get { return GetAttributeValueAsInt("index"); } set { SetAttributeValue("index", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(index))]
    public class index : Tag
    {
        public index() : base(Namespace.index) { }
        public index(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(last))]
    public class last : Tag
    {
        public last() : base(Namespace.last) { }
        public last(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(max))]
    public class max : Tag
    {
        public max() : base(Namespace.max) { }
        public max(XElement other) : base(other) { }
    }
}

/*
 * <?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/rsm'
    xmlns='http://jabber.org/protocol/rsm'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0059: http://www.xmpp.org/extensions/xep-0059.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='set'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='after' type='xs:string' minOccurs='0' maxOccurs='1'/>
        <xs:element name='before' type='xs:string' minOccurs='0' maxOccurs='1'/>
        <xs:element name='count' type='xs:int' minOccurs='0' maxOccurs='1'/>
        <xs:element ref='first' minOccurs='0' maxOccurs='1'/>
        <xs:element name='index' type='xs:int' minOccurs='0' maxOccurs='1'/>
        <xs:element name='last' type='xs:string' minOccurs='0' maxOccurs='1'/>
        <xs:element name='max' type='xs:int' minOccurs='0' maxOccurs='1'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='first'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='xs:string'>
          <xs:attribute name='index' type='xs:int' use='optional'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

</xs:schema>
*/
