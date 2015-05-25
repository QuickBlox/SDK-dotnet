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

namespace XMPP.tags.jabber.features.compress
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/features/compress";
        public static XName compression = XName.Get("compression", Name);
        public static XName method = XName.Get("method", Name);
        public static XName compress = XName.Get("compress", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(compression))]
	public class compression : Tag
	{
        public compression() : base(Namespace.compression) {} 
        public compression(XElement other) : base(other) {}

        public IEnumerable<method> methodElements { get { return Elements<method>(Namespace.method); } }
		public string[] methods { 
            get 
            { 
                List<string> methods = new List<string>();
                foreach( var method in methodElements)
                    methods.Add(method.Value);
                
                return methods.ToArray();
            }
        }
	}


    [XMPPTag(typeof(Namespace), typeof(method))]
    public class method : Tag
    {
        public method() : base(Namespace.method) {} 
        public method(XElement other) : base(other) {}
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/features/compress'
    xmlns='http://jabber.org/features/compress'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0138: http://www.xmpp.org/extensions/xep-0138.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='compression'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='method' type='xs:NCName' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>
*/


