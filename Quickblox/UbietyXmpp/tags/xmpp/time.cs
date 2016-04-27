// time.cs
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

namespace XMPP.tags.xmpp.time
{
    public class Namespace
    {
        public static string Name = "urn:xmpp:time";
        public static XName time = XName.Get("time", Name);
        public static XName tzo = XName.Get("tzo", Name);
        public static XName utc = XName.Get("utc", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(time))]
    public class time : Tag
    {
        public time() : base(Namespace.time) {} 
        public time(XElement other) : base(other) {}

        public IEnumerable<tzo> tzoElements { get { return Elements<tzo>(Namespace.tzo); } }
        public IEnumerable<utc> utcElements { get { return Elements<utc>(Namespace.utc); } }
    }

    [XMPPTag(typeof(Namespace), typeof(tzo))]
    public class tzo : Tag
    {
        public tzo() : base(Namespace.tzo) {} 
        public tzo(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(utc))]
    public class utc : Tag
    {
        public utc() : base(Namespace.utc) {} 
        public utc(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:time'
    xmlns='urn:xmpp:time'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0202: http://www.xmpp.org/extensions/xep-0202.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='time'>
    <xs:complexType>
      <xs:sequence minOccurs='0'>
        <xs:element name='tzo' type='xs:string'/>
        <xs:element name='utc' type='xs:string'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/