// auth.cs
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

namespace XMPP.tags.jabber.iq.auth
{
    public class Namespace
    {
        public static string Name = "jabber:iq:auth";
        public static XName query = XName.Get("query", Name);
        public static XName username = XName.Get("username", Name);
        public static XName password = XName.Get("password", Name);
        public static XName digest = XName.Get("digest", Name);
        public static XName resource = XName.Get("resource", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {} 

        public IEnumerable<username> usernameElements { get { return Elements<username>(Namespace.username); } }
        public IEnumerable<password> passwordElements { get { return Elements<password>(Namespace.password); } }
        public IEnumerable<digest> digestElements { get { return Elements<digest>(Namespace.digest); } }
        public IEnumerable<resource> resourceElements { get { return Elements<resource>(Namespace.resource); } }
    }

    [XMPPTag(typeof(Namespace), typeof(username))]
    public class username : Tag
    {
        public username() : base(Namespace.username) {} 
        public username(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(password))]
    public class password : Tag
    {
        public password() : base(Namespace.password) {} 
        public password(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(digest))]
    public class digest : Tag
    {
        public digest() : base(Namespace.digest) {} 
        public digest(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(resource))]
    public class resource : Tag
    {
        public resource() : base(Namespace.resource) {} 
        public resource(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:auth'
    xmlns='jabber:iq:auth'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      NOTE WELL: Non-SASL Authentication via the jabber:iq:auth
      protocol has been superseded by SASL Authentication as 
      defined in RFC 3920, and is now obsolete.

      For historical purposes, the protocol documented by this 
      schema is defined in XEP-0078: 

      http://www.xmpp.org/extensions/xep-0078.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='username' type='xs:string' minOccurs='0'/>
        <xs:choice>
          <xs:element name='password' type='xs:string' minOccurs='0'/>
          <xs:element name='digest' type='xs:string' minOccurs='0'/>
        </xs:choice>
        <xs:element name='resource' type='xs:string' minOccurs='0'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/