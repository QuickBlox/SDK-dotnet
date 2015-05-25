// version.cs
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

namespace XMPP.tags.jabber.iq.version
{
    public class Namespace
    {
        public static string Name = "jabber:iq:version";
        public static XName query = XName.Get("query", Name);
        public static XName name = XName.Get("name", Name);
        public static XName version = XName.Get("version", Name);
        public static XName os = XName.Get("os", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {}

        public IEnumerable<name>    nameElements { get { return Elements<name>(Namespace.name); } }
        public IEnumerable<version>    versionElements { get { return Elements<version>(Namespace.version); } }
        public IEnumerable<os>    osElements { get { return Elements<os>(Namespace.os); } }
    }

    [XMPPTag(typeof(Namespace), typeof(name))]
    public class name : Tag
    {
        public name() : base(Namespace.name) { }
        public name(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(version))]
    public class version : Tag
    {
        public version() : base(Namespace.version) { }
        public version(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(os))]
    public class os : Tag
    {
        public os() : base(Namespace.os) { }
        public os(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:version'
    xmlns='jabber:iq:version'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0092: http://www.xmpp.org/extensions/xep-0092.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:sequence minOccurs='0'>
        <xs:element name='name' type='xs:string' minOccurs='1'/>
        <xs:element name='version' type='xs:string' minOccurs='1'/>
        <xs:element name='os' type='xs:string' minOccurs='0'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/