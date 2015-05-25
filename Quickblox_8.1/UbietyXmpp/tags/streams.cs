// streams.cs
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
using System.Text;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.streams
{
    public class Namespace
    {
        public static string Name = "http://etherx.jabber.org/streams";
        public static XName features = XName.Get("features", Name);
        public static XName stream = XName.Get("stream", Name);
        public static XName error = XName.Get("error", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(stream))]
    public class stream : Tag
    {
        public stream() : base(Namespace.stream) {} 
        public stream(XElement other) : base(other) {}

		public string from { get { return (string)GetAttributeValue("from"); } set { SetAttributeValue("from", value); } }
		public string to { get { return (string)GetAttributeValue("to"); } set { SetAttributeValue("to", value); } }
		public string id { get { return (string)GetAttributeValue("id"); } set { SetAttributeValue("id", value); } }
        public string lang { get { return (string)GetAttributeValue(XName.Get("lang", xml.Namespace.Name)); } set { SetAttributeValue(XName.Get("lang", xml.Namespace.Name), value); } }
		public string version { get { return (string)GetAttributeValue("version"); } set { SetAttributeValue("version", value); } }
		public string xmlns { get { return (string)GetAttributeValue("xmlns"); } set { SetAttributeValue("xmlns", value); } }

		public features features { get { return Element<features>(Namespace.features); } }
        public IEnumerable<jabber.client.message> bodyElements { get { return Elements<jabber.client.message>(jabber.client.Namespace.message); } }
        public IEnumerable<jabber.client.presence> presenceElements { get { return Elements<jabber.client.presence>(jabber.client.Namespace.presence); } }
        public IEnumerable<jabber.client.iq> iqElements { get { return Elements<jabber.client.iq>(jabber.client.Namespace.iq); } }
        public IEnumerable<error> errorElements { get { return Elements<error>(Namespace.error); } }

        public string StartTag
        {
            get
            {
                var sb = new StringBuilder("<");
                sb.Append(Name.LocalName);
                sb.Append(":");
                sb.Append(Name.LocalName);
                sb.Append(" xmlns");
                sb.Append(":");
                sb.Append(Name.LocalName);                
                sb.Append("=\'");
                sb.Append(Name.NamespaceName);
                sb.Append("\'");

                foreach (XAttribute attr in Attributes())
                {
                    sb.Append(" ");
                    sb.Append(attr.Name.LocalName);
                    sb.Append("=\'");
                    sb.Append(attr.Value);
                    sb.Append("\'");
                }
                sb.Append(">");
                return sb.ToString();
            }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(features))]
    public class features : Tag
    {

        public features() : base(Namespace.features) {  }
        public features(XElement other) : base(other) { }

        public xmpp_sasl.mechanisms mechanisms { get { return Element<xmpp_sasl.mechanisms>(xmpp_sasl.Namespace.mechanisms); } }
        public xmpp_tls.starttls starttls { get { return Element<xmpp_tls.starttls>(xmpp_tls.Namespace.starttls); } }
        public jabber.features.compress.compression compression { get { return Element<jabber.features.compress.compression>(jabber.features.compress.Namespace.compression); } }
    }


    [XMPPTag(typeof(Namespace), typeof(error))]
    public class error : Tag
    {
        public error() : base(Namespace.error) {} 
        public error(XElement other) : base(other) {}
    }

}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://etherx.jabber.org/streams'
    xmlns='http://etherx.jabber.org/streams'
    elementFormDefault='unqualified'>

  <xs:import namespace='jabber:client'
             schemaLocation='http://xmpp.org/schemas/jabber-client.xsd'/>
  <xs:import namespace='jabber:server'
             schemaLocation='http://xmpp.org/schemas/jabber-server.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-sasl'
             schemaLocation='http://xmpp.org/schemas/sasl.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-streams'
             schemaLocation='http://xmpp.org/schemas/streamerror.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-tls'
             schemaLocation='http://xmpp.org/schemas/tls.xsd'/>
  <xs:import namespace='http://www.w3.org/XML/1998/namespace'
             schemaLocation='http://www.w3.org/2001/03/xml.xsd'/>

  <xs:element name='stream'>
    <xs:complexType>
      <xs:sequence xmlns:client='jabber:client'
                   xmlns:server='jabber:server'>
        <xs:element ref='features' 
                    minOccurs='0' 
                    maxOccurs='1'/>
        <xs:any namespace='urn:ietf:params:xml:ns:xmpp-tls'
                minOccurs='0'
                maxOccurs='1'/>
        <xs:any namespace='urn:ietf:params:xml:ns:xmpp-sasl'
                minOccurs='0'
                maxOccurs='1'/>
        <xs:any namespace='##other'
                minOccurs='0'
                maxOccurs='unbounded'
                processContents='lax'/>
        <xs:choice minOccurs='0' maxOccurs='1'>
          <xs:choice minOccurs='0' maxOccurs='unbounded'>
            <xs:element ref='client:message'/>
            <xs:element ref='client:presence'/>
            <xs:element ref='client:iq'/>
          </xs:choice>
          <xs:choice minOccurs='0' maxOccurs='unbounded'>
            <xs:element ref='server:message'/>
            <xs:element ref='server:presence'/>
            <xs:element ref='server:iq'/>
          </xs:choice>
        </xs:choice>
        <xs:element ref='error' minOccurs='0' maxOccurs='1'/>
      </xs:sequence>
      <xs:attribute name='from' type='xs:string' use='optional'/>
      <xs:attribute name='id' type='xs:string' use='optional'/>
      <xs:attribute name='to' type='xs:string' use='optional'/>
      <xs:attribute name='version' type='xs:decimal' use='optional'/>
      <xs:attribute ref='xml:lang' use='optional'/>
      <xs:anyAttribute namespace='##other' processContents='lax'/> 
    </xs:complexType>
  </xs:element>

  <xs:element name='features'>
    <xs:complexType>
      <xs:sequence>
        <xs:any namespace='##other'
                minOccurs='0'
                maxOccurs='unbounded'
                processContents='lax'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='error'>
    <xs:complexType>
      <xs:sequence  xmlns:err='urn:ietf:params:xml:ns:xmpp-streams'>
        <xs:group   ref='err:streamErrorGroup'/>
        <xs:element ref='err:text'
                    minOccurs='0'
                    maxOccurs='1'/>
        <xs:any     namespace='##other'
                    minOccurs='0'
                    maxOccurs='1'
                    processContents='lax'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/