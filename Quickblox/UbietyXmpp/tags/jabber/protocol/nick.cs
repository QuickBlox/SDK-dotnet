// nick.cs
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

namespace XMPP.tags.jabber.protocol.nick
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/nick";
        public static XName nick = XName.Get("nick", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(nick))]
    public class nick : Tag
    {
        public nick() : base(Namespace.nick) { }
        public nick(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/nick'
    xmlns='http://jabber.org/protocol/nick'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0172: http://www.xmpp.org/extensions/xep-0172.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='nick' type='xs:string'/>

</xs:schema>

*/