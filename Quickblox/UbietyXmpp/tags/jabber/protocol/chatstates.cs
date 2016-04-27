// chatstates.cs
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

namespace XMPP.tags.jabber.protocol.chatstates
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/chatstates";
        public static XName active = XName.Get("active", Name);
        public static XName composing = XName.Get("composing", Name);
        public static XName gone = XName.Get("gone", Name);
        public static XName inactive = XName.Get("inactive", Name);
        public static XName paused = XName.Get("paused", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(active))]
    public class active : Tag
    {
        public active() : base(Namespace.active) { }
        public active(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(composing))]
    public class composing : Tag
    {
        public composing() : base(Namespace.composing) { }
        public composing(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(gone))]
    public class gone : Tag
    {
        public gone() : base(Namespace.gone) { }
        public gone(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(inactive))]
    public class inactive : Tag
    {
        public inactive() : base(Namespace.inactive) { }
        public inactive(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(paused))]
    public class paused : Tag
    {
        public paused() : base(Namespace.paused) { }
        public paused(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/chatstates'
    xmlns='http://jabber.org/protocol/chatstates'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0085: http://www.xmpp.org/extensions/xep-0085.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='active' type='empty'/>
  <xs:element name='composing' type='empty'/>
  <xs:element name='gone' type='empty'/>
  <xs:element name='inactive' type='empty'/>
  <xs:element name='paused' type='empty'/>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/