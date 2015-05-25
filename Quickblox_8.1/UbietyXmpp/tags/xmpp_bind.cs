// xmpp_bind.cs
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

using System;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.xmpp_bind
{
    public class Namespace
    {
        public static string Name = "urn:ietf:params:xml:ns:xmpp-bind";
        public static XName bind = XName.Get("bind", Name);
        public static XName jid = XName.Get("jid", Name);
        public static XName resource = XName.Get("resource", Name);
        public static XName required = XName.Get("required", Name);
        public static XName optional = XName.Get("optional", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(bind))]
    public class bind : Tag
    {
        public bind() : base(Namespace.bind) { }
        public bind(XElement other) : base(other) { }

		public jid jid { get { return Element<jid>(Namespace.jid); } }
		public required required { get { return Element<required>(Namespace.required); } }
		public optional optional { get { return Element<optional>(Namespace.optional); } }
    }

    [XMPPTag(typeof(Namespace), typeof(jid))]
    public class jid : Tag
    {
        public jid() : base(Namespace.jid) { }
        public jid(XElement other) : base(other) { }

        public new string Value { get { return base.Value; } set { if (value.Length < 8 || value.Length > 3071) { throw new Exception("Text out of range"); } base.Value = value; } } // fullJIDType
        public JID JID { get { return new JID(base.Value); } set { base.Value = value.ToString(); } }
    }

    [XMPPTag(typeof(Namespace), typeof(resource))]
    public class resource : Tag
    {
        public resource() : base(Namespace.resource) { }
        public resource(XElement other) : base(other) { }

        public new string Value { get { return base.Value; } set { if (value.Length < 1 || value.Length > 1023) { throw new Exception("Text out of range"); } base.Value = value; } } // resourceType
    }

    [XMPPTag(typeof(Namespace), typeof(required))]
    public class required : Tag 
    {
        public required() : base(Namespace.required) { }
        public required(XElement other) : base(other) { } 
    }

    [XMPPTag(typeof(Namespace), typeof(optional))]
    public class optional : Tag 
    {
        public optional() : base(Namespace.optional) { }
        public optional(XElement other) : base(other) { } 
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:ietf:params:xml:ns:xmpp-bind'
    xmlns='urn:ietf:params:xml:ns:xmpp-bind'
    elementFormDefault='qualified'>

  <xs:element name='bind'>
    <xs:complexType>
      <xs:choice>
        <xs:element name='resource' type='resourceType'/>
        <xs:element name='jid' type='fullJIDType'/>
      </xs:choice>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='fullJIDType'>
    <xs:restriction base='xs:string'>
      <xs:minLength value='8'/>
      <xs:maxLength value='3071'/>
    </xs:restriction>
  </xs:simpleType>

  <xs:simpleType name='resourceType'>
    <xs:restriction base='xs:string'>
      <xs:minLength value='1'/>
      <xs:maxLength value='1023'/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/