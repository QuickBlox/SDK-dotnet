// update.cs
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

namespace XMPP.tags.vcard_temp.x.update
{
    public class Namespace
    {
        public static string Name = "vcard-temp:x:update";
        public static XName x = XName.Get("x", Name);
        public static XName photo = XName.Get("photo", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(x))]
    public class x : Tag
    {
        public x() : base(Namespace.x) {} 
        public x(XElement other) : base(other) {}
        public IEnumerable<photo> photoElements { get { return Elements<photo>(Namespace.photo); } }
    }

    [XMPPTag(typeof(Namespace), typeof(photo))]
    public class photo : Tag
    {
        public photo() : base(Namespace.photo) {} 
        public photo(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='vcard-temp:x:update'
    xmlns='vcard-temp:x:update'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0153: http://www.xmpp.org/extensions/xep-0153.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='x'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='photo' minOccurs='0' type='xs:base64Binary'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/