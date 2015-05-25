// compress.cs
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

namespace XMPP.tags.jabber.protocol.compress
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/compress";
        public static XName compress = XName.Get("compress", Name);
        public static XName method = XName.Get("method", Name);
        public static XName compressed = XName.Get("compressed", Name);

        public static XName failure = XName.Get("failure", Name);
        public static XName setup_failed = XName.Get("setup-failed", Name);
        public static XName processing_failed = XName.Get("processing-failed", Name);
        public static XName unsupported_method = XName.Get("unsupported-method", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(compress))]
    public class compress : Tag
    {
        public compress() : base(Namespace.compress) {} 
        public compress(XElement other) : base(other) {}

        public IEnumerable<method> methodElements { get { return Elements<method>(Namespace.method); } }
    }

    [XMPPTag(typeof(Namespace), typeof(method))]
    public class method : Tag
    {
        public method() : base(Namespace.method) {} 
        public method(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(compressed))]
    public class compressed : Tag
    { 
        public compressed() : base(Namespace.compressed) {} 
        public compressed(XElement other) : base(other) {}
    }

    [XMPPTag(typeof(Namespace), typeof(failure))]
    public class failure : Tag 
    { 
        public failure() : base(Namespace.failure) {} 
        public failure(XElement other) : base(other) {}

		public setup_failed setup_failed { get { return Element<setup_failed>(Namespace.setup_failed); } }
		public processing_failed processing_failed { get { return Element<processing_failed>(Namespace.processing_failed); } }
		public unsupported_method unsupported_method { get { return Element<unsupported_method>(Namespace.unsupported_method); } }
    }

    [XMPPTag(typeof(Namespace), typeof(setup_failed))]
    public class setup_failed : Tag
    {
        public setup_failed() : base(Namespace.setup_failed) { }
        public setup_failed(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(processing_failed))]
    public class processing_failed : Tag
    {
        public processing_failed() : base(Namespace.processing_failed) { }
        public processing_failed(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(unsupported_method))]
    public class unsupported_method : Tag
    {
        public unsupported_method() : base(Namespace.unsupported_method) { }
        public unsupported_method(XElement other) : base(other) { }
    }


}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/compress'
    xmlns='http://jabber.org/protocol/compress'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0138: http://www.xmpp.org/extensions/xep-0138.html
    </xs:documentation>
  </xs:annotation>

  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-stanzas'
             schemaLocation='http://xmpp.org/schemas/stanzaerror.xsd'/>

  <xs:element name='compress'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='method' type='xs:NCName' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='compressed' type='empty'/>

  <xs:element name='failure'>
    <xs:complexType>
      <xs:choice>
        <xs:element name='setup-failed' type='empty'/>
        <xs:element name='processing-failed' type='empty'/>
        <xs:element name='unsupported-method' type='empty'/>
        <xs:sequence xmlns:err='urn:ietf:params:xml:ns:xmpp-stanzas'>
          <xs:group ref='err:stanzaErrorGroup'/>
          <xs:element ref='err:text' minOccurs='0'/>
        </xs:sequence>
      </xs:choice>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/