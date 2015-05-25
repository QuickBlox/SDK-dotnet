// archive.cs
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

namespace XMPP.tags.xmpp.archive
{
    public class Namespace
    {
        public static string Name = "urn:xmpp:archive";
        public static XName auto = XName.Get("auto", Name);
        public static XName changed = XName.Get("changed", Name);
        public static XName chat = XName.Get("chat", Name);
        public static XName body = XName.Get("body", Name);
        public static XName default_ = XName.Get("default", Name);
        public static XName feature = XName.Get("feature", Name);
        public static XName item = XName.Get("item", Name);
        public static XName list = XName.Get("list", Name);
        public static XName method = XName.Get("method", Name);
        public static XName modified = XName.Get("modified", Name);
        public static XName removed = XName.Get("removed", Name);
        public static XName note = XName.Get("note", Name);
        public static XName pref = XName.Get("pref", Name);
        public static XName itemremove = XName.Get("itemremove", Name);
        public static XName remove = XName.Get("remove", Name);
        public static XName retrieve = XName.Get("retrieve", Name);
        public static XName save = XName.Get("save", Name);
        public static XName from = XName.Get("from", Name);
        public static XName next = XName.Get("next", Name);
        public static XName previous = XName.Get("previous", Name);
        public static XName to = XName.Get("to", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(auto))] 
    public class auto : Tag 
    { 
        public auto() : base(Namespace.auto) {} 
        public auto(XElement other) : base(other) {} 

		public string save { get { return (string)GetAttributeValue("save"); } set { SetAttributeValue("save", value); } }

    }

    [XMPPTag(typeof(Namespace), typeof(changed))] 
    public class changed : Tag 
    { 
        public changed() : base(Namespace.changed) {} 
        public changed(XElement other) : base(other) {} 

		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }
		public string version { get { return (string)GetAttributeValue("version"); } set { SetAttributeValue("version", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(chat))] 
    public class chat : Tag 
    { 
        public chat() : base(Namespace.chat) {} 
        public chat(XElement other) : base(other) {} 

		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string subject { get { return (string)GetAttributeValue("subject"); } set { SetAttributeValue("subject", value); } }
		public string thread { get { return (string)GetAttributeValue("thread"); } set { SetAttributeValue("thread", value); } }
		public string version { get { return (string)GetAttributeValue("version"); } set { SetAttributeValue("version", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }

		public from from { get { return Element<from>(Namespace.from); } }
		public next next { get { return Element<next>(Namespace.next); } }
		public previous previous { get { return Element<previous>(Namespace.previous); } }
		public to to { get { return Element<to>(Namespace.to); } }
    }

    [XMPPTag(typeof(Namespace), typeof(body))] 
    public class body : Tag 
    { 
        public body() : base(Namespace.body) {} 
        public body(XElement other) : base(other) {} 
    }

    [XMPPTag(typeof(Namespace), typeof(default_))] 
    public class default_ : Tag 
    { 
        public default_() : base(Namespace.default_) {} 
        public default_(XElement other) : base(other) {}

        public enum otrEnum
        {
            none,
            approve,
            concede,
            forbid,
            oppose,
            prefer,
            require
        }

        public enum saveEnum
        {
            none,
            body,
            @false,
            message,
            stream
        }

		public string expire { get { return (string)GetAttributeValue("expire"); } set { SetAttributeValue("expire", value); } }
		public string unset { get { return (string)GetAttributeValue("unset"); } set { SetAttributeValue("unset", value); } }

        public otrEnum otr { get { return GetAttributeEnum<otrEnum>("otr"); } set { SetAttributeEnum<otrEnum>("otr", value); } }
        public saveEnum save { get { return GetAttributeEnum<saveEnum>("save"); } set { SetAttributeEnum<saveEnum>("save", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(feature))] 
    public class feature : Tag 
    { 
        public feature() : base(Namespace.feature) {} 
        public feature(XElement other) : base(other) {}

		public default_ default_ { get { return Element<default_>(Namespace.default_); } }
    }

    [XMPPTag(typeof(Namespace), typeof(item))] 
    public class item : Tag 
    { 
        public item() : base(Namespace.item) {} 
        public item(XElement other) : base(other) {}

        public enum otrEnum
        {
            none,
            approve,
            concede,
            forbid,
            oppose,
            prefer,
            require
        }

        public enum saveEnum
        {
            none,
            body,
            @false,
            message,
            stream
        }

		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string expire { get { return (string)GetAttributeValue("expire"); } set { SetAttributeValue("expire", value); } }
		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }

        public otrEnum otr { get { return GetAttributeEnum<otrEnum>("otr"); } set { SetAttributeEnum<otrEnum>("otr", value); } }
        public saveEnum save { get { return GetAttributeEnum<saveEnum>("save"); } set { SetAttributeEnum<saveEnum>("save", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(list))] 
    public class list : Tag 
    { 
        public list() : base(Namespace.list) {} 
        public list(XElement other) : base(other) {}

		public string end { get { return (string)GetAttributeValue("end"); } set { SetAttributeValue("end", value); } }
		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }

        public IEnumerable<chat> chatElements { get { return Elements<chat>(Namespace.chat); } }
    }

    [XMPPTag(typeof(Namespace), typeof(method))] 
    public class method : Tag 
    { 
        public method() : base(Namespace.method) {} 
        public method(XElement other) : base(other) {} 

        public enum useEnum
        {
            none,
            concede,
            forbid,
            prefer
        }

		public string type { get { return (string)GetAttributeValue("type"); } set { SetAttributeValue("type", value); } }
        public useEnum use { get { return GetAttributeEnum<useEnum>("use"); } set { SetAttributeEnum<useEnum>("use", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(modified))] 
    public class modified : Tag 
    { 
        public modified() : base(Namespace.modified) {} 
        public modified(XElement other) : base(other) {}

		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }

        public IEnumerable<changed> changedElements { get { return Elements<changed>(Namespace.changed); } }
        public IEnumerable<removed> removedElements { get { return Elements<removed>(Namespace.removed); } }
    }

    [XMPPTag(typeof(Namespace), typeof(note))] 
    public class note : Tag 
    { 
        public note() : base(Namespace.note) {} 
        public note(XElement other) : base(other) {}

		public string utc { get { return (string)GetAttributeValue("utc"); } set { SetAttributeValue("utc", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(pref))] 
    public class pref : Tag 
    { 
        public pref() : base(Namespace.pref) {} 
        public pref(XElement other) : base(other) {}

        public IEnumerable<auto> autoElements { get { return Elements<auto>(Namespace.auto); } }
        public IEnumerable<default_> default_Elements { get { return Elements<default_>(Namespace.default_); } }
        public IEnumerable<item> itemElements { get { return Elements<item>(Namespace.item); } }
        public IEnumerable<method> methodElements { get { return Elements<method>(Namespace.method); } }
    }

    [XMPPTag(typeof(Namespace), typeof(itemremove))] 
    public class itemremove : Tag 
    { 
        public itemremove() : base(Namespace.itemremove) {} 
        public itemremove(XElement other) : base(other) {}

        public IEnumerable<item> itemElements { get { return Elements<item>(Namespace.item); } }
    }

    [XMPPTag(typeof(Namespace), typeof(remove))] 
    public class remove : Tag 
    { 
        public remove() : base(Namespace.remove) {} 
        public remove(XElement other) : base(other) {}

		public string end { get { return (string)GetAttributeValue("end"); } set { SetAttributeValue("end", value); } }
		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string open { get { return (string)GetAttributeValue("open"); } set { SetAttributeValue("open", value); } }
		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(removed))] 
    public class removed : Tag 
    { 
        public removed() : base(Namespace.removed) {} 
        public removed(XElement other) : base(other) {}

		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }
		public string version { get { return (string)GetAttributeValue("version"); } set { SetAttributeValue("version", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(retrieve))] 
    public class retrieve : Tag 
    { 
        public retrieve() : base(Namespace.retrieve) {} 
        public retrieve(XElement other) : base(other) {}

		public string exactmatch { get { return (string)GetAttributeValue("exactmatch"); } set { SetAttributeValue("exactmatch", value); } }
		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }
    }
    
    [XMPPTag(typeof(Namespace), typeof(save))] 
    public class save : Tag 
    { 
        public save() : base(Namespace.save) {} 
        public save(XElement other) : base(other) {}

        public chat chat { get { return Element<chat>(Namespace.chat); } }
    }

    [XMPPTag(typeof(Namespace), typeof(from))] 
    public class from : Tag 
    { 
        public from() : base(Namespace.from) {} 
        public from(XElement other) : base(other) {} 

		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }
		public string secs { get { return (string)GetAttributeValue("secs"); } set { SetAttributeValue("secs", value); } }
		public string utc { get { return (string)GetAttributeValue("utc"); } set { SetAttributeValue("utc", value); } }

        public IEnumerable<body> bodyElements { get { return Elements<body>(Namespace.body); } }
    }

    [XMPPTag(typeof(Namespace), typeof(next))] 
    public class next : Tag 
    { 
        public next() : base(Namespace.next) {} 
        public next(XElement other) : base(other) {} 

		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }

    }

    [XMPPTag(typeof(Namespace), typeof(previous))] 
    public class previous : Tag 
    { 
        public previous() : base(Namespace.previous) {} 
        public previous(XElement other) : base(other) {}

		public string start { get { return (string)GetAttributeValue("start"); } set { SetAttributeValue("start", value); } }
		public string with { get { return (string)GetAttributeValue("with"); } set { SetAttributeValue("with", value); } }
    }

    [XMPPTag(typeof(Namespace), typeof(to))] 
    public class to : Tag 
    { 
        public to() : base(Namespace.to) {} 
        public to(XElement other) : base(other) {}

		public string jid { get { return (string)GetAttributeValue("jid"); } set { SetAttributeValue("jid", value); } }
		public string name { get { return (string)GetAttributeValue("name"); } set { SetAttributeValue("name", value); } }
		public string secs { get { return (string)GetAttributeValue("secs"); } set { SetAttributeValue("secs", value); } }
		public string utc { get { return (string)GetAttributeValue("utc"); } set { SetAttributeValue("utc", value); } }

        public IEnumerable<body> bodyElements { get { return Elements<body>(Namespace.body); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:archive'
    xmlns='urn:xmpp:archive'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0136: http://www.xmpp.org/extensions/xep-0136.html
    </xs:documentation>
  </xs:annotation>

  <xs:annotation>
    <xs:documentation>
      The allowable root elements for the namespace defined
      herein are:
        - auto
        - chat
        - itemremove
        - list
        - modified
        - pref
        - remove
        - retrieve
        - save
    </xs:documentation>
  </xs:annotation>

  <xs:element name='auto'>
    <xs:complexType>
      <xs:sequence>
        <xs:any processContents='lax' namespace='##other' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='save' type='xs:boolean' use='required'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='changed'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
          <xs:attribute name='start' type='xs:dateTime' use='required'/>
          <xs:attribute name='with' type='xs:string' use='required'/>
          <xs:attribute name='version' type='xs:nonNegativeInteger' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='chat'>
    <xs:complexType>
      <xs:choice minOccurs='0' maxOccurs='unbounded'>
        <xs:element name='from' type='messageType'/>
        <xs:element name='next' type='linkType'/>
        <xs:element ref='note'/>
        <xs:element name='previous' type='linkType'/>
        <xs:element name='to' type='messageType'/>
        <xs:any processContents='lax' namespace='##other'/>
      </xs:choice>
      <xs:attribute name='start' type='xs:dateTime' use='required'/>
      <xs:attribute name='subject' type='xs:string' use='optional'/>
      <xs:attribute name='thread' use='optional' type='xs:string'/>
      <xs:attribute name='version' use='optional' type='xs:nonNegativeInteger'/>
      <xs:attribute name='with' type='xs:string' use='required'/>
    </xs:complexType>
  </xs:element>

  <xs:complexType name='messageType'>
    <xs:sequence>
      <xs:element name='body' type='xs:string' minOccurs='0' maxOccurs='unbounded'/>
      <xs:any processContents='lax' namespace='##other' minOccurs='0' maxOccurs='unbounded'/>
    </xs:sequence>
    <xs:attribute name='jid' type='xs:string' use='optional'/>
    <xs:attribute name='name' type='xs:string' use='optional'/>
    <xs:attribute name='secs' type='xs:nonNegativeInteger' use='optional'/>
    <xs:attribute name='utc' type='xs:dateTime' use='optional'/>
  </xs:complexType>

  <xs:complexType name='linkType'>
    <xs:simpleContent>
      <xs:extension base='empty'>
        <xs:attribute name='start' type='xs:dateTime' use='optional'/>
        <xs:attribute name='with' type='xs:string' use='optional'/>
      </xs:extension>
    </xs:simpleContent>
  </xs:complexType>

  <xs:element name='default'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='expire' type='xs:nonNegativeInteger' use='optional'/>
          <xs:attribute name='otr' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='approve'/>
                <xs:enumeration value='concede'/>
                <xs:enumeration value='forbid'/>
                <xs:enumeration value='oppose'/>
                <xs:enumeration value='prefer'/>
                <xs:enumeration value='require'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
          <xs:attribute name='save' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='body'/>
                <xs:enumeration value='false'/>
                <xs:enumeration value='message'/>
                <xs:enumeration value='stream'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
          <xs:attribute name='unset' use='optional' type='xs:boolean'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='feature'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='default' minOccurs='0' maxOccurs='1'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='item'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
          <xs:attribute name='expire' type='xs:nonNegativeInteger' use='optional'/>
          <xs:attribute name='jid' use='required' type='xs:string'/>
          <xs:attribute name='otr' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='approve'/>
                <xs:enumeration value='concede'/>
                <xs:enumeration value='forbid'/>
                <xs:enumeration value='oppose'/>
                <xs:enumeration value='prefer'/>
                <xs:enumeration value='require'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
          <xs:attribute name='save' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='body'/>
                <xs:enumeration value='false'/>
                <xs:enumeration value='message'/>
                <xs:enumeration value='stream'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='list'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='chat' minOccurs='0' maxOccurs='unbounded'/>
        <xs:any processContents='lax' namespace='##other' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='end' type='xs:dateTime' use='optional'/>
      <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
      <xs:attribute name='start' type='xs:dateTime' use='optional'/>
      <xs:attribute name='with' type='xs:string' use='optional'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='method'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='type' type='xs:string' use='required'/>
          <xs:attribute name='use' use='required'>
            <xs:simpleType>
              <xs:restriction base='xs:NCName'>
                <xs:enumeration value='concede'/>
                <xs:enumeration value='forbid'/>
                <xs:enumeration value='prefer'/>
              </xs:restriction>
            </xs:simpleType>
          </xs:attribute>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='modified'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='changed' minOccurs='0' maxOccurs='unbounded'/>
        <xs:element ref='removed' minOccurs='0' maxOccurs='unbounded'/>
        <xs:any processContents='lax' namespace='##other' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='start' type='xs:dateTime' use='required'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='note'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='xs:string'>
          <xs:attribute name='utc' type='xs:dateTime' use='optional'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='pref'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='auto' minOccurs='0' maxOccurs='1'/>
        <xs:element ref='default' minOccurs='0' maxOccurs='1'/>
        <xs:element ref='item' minOccurs='0' maxOccurs='unbounded'/>
        <xs:element ref='method' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='itemremove'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='item' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='remove'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='end' type='xs:dateTime' use='optional'/>
          <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
          <xs:attribute name='open' use='optional' type='xs:boolean'/>
          <xs:attribute name='start' type='xs:dateTime' use='required'/>
          <xs:attribute name='with' type='xs:string' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='removed'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
          <xs:attribute name='start' type='xs:dateTime' use='required'/>
          <xs:attribute name='with' type='xs:string' use='required'/>
          <xs:attribute name='version' type='xs:nonNegativeInteger' use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='retrieve'>
    <xs:complexType>
      <xs:sequence>
        <xs:any processContents='lax' namespace='##other' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='exactmatch' type='xs:boolean' use='optional'/>
      <xs:attribute name='start' type='xs:dateTime' use='required'/>
      <xs:attribute name='with' type='xs:string' use='required'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='save'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='chat' minOccurs='1' maxOccurs='1'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/