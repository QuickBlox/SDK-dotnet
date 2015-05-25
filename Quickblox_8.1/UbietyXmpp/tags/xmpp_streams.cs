// xmpp_streams.cs
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


namespace XMPP.tags.xmpp_streams
{
    public class Namespace
    {
        public static string Name = "urn:ietf:params:xml:ns:xmpp-streams";
        public static XName bad_format = XName.Get("bad-format", Name);
        public static XName bad_namespace_prefix = XName.Get("bad-namespace-prefix", Name);
        public static XName conflict = XName.Get("conflict", Name);
        public static XName connection_timeout = XName.Get("connection-timeout", Name);
        public static XName host_gone = XName.Get("host-gone", Name);
        public static XName host_unknown = XName.Get("host-unknown", Name);
        public static XName improper_addressing = XName.Get("improper-addressing", Name);
        public static XName internal_server_error = XName.Get("internal-server-error", Name);
        public static XName invalid_from = XName.Get("invalid-from", Name);
        public static XName invalid_id = XName.Get("invalid-id", Name);
        public static XName invalid_namespace = XName.Get("invalid-namespace", Name);
        public static XName invalid_xml = XName.Get("invalid-xml", Name);
        public static XName not_authorized = XName.Get("not-authorized", Name);
        public static XName policy_violation = XName.Get("policy-violation", Name);
        public static XName remote_connection_failed = XName.Get("remote-connection-failed", Name);
        public static XName reset = XName.Get("reset", Name);
        public static XName resource_constraint = XName.Get("resource-constraint", Name);
        public static XName restricted_xml = XName.Get("restricted-xml", Name);
        public static XName see_other_host = XName.Get("see-other-host", Name);
        public static XName system_shutdown = XName.Get("system-shutdown", Name);
        public static XName undefined_condition = XName.Get("undefined-condition", Name);
        public static XName unsupported_encoding = XName.Get("unsupported-encoding", Name);
        public static XName unsupported_stanza_type = XName.Get("unsupported-stanza-type", Name);
        public static XName unsupported_version = XName.Get("unsupported-version", Name);
        public static XName xml_not_well_formed = XName.Get("xml-not-well-formed", Name);
        public static XName text = XName.Get("text", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(bad_format))]
    public class bad_format : Tag
	{
		 public bad_format() : base(Namespace.bad_format) {} 
		 public bad_format(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(bad_namespace_prefix))]
    public class bad_namespace_prefix : Tag
	{
		 public bad_namespace_prefix() : base(Namespace.bad_namespace_prefix) {} 
		 public bad_namespace_prefix(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(conflict))]
    public class conflict : Tag
	{
		 public conflict() : base(Namespace.conflict) {} 
		 public conflict(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(connection_timeout))]
    public class connection_timeout : Tag
	{
		 public connection_timeout() : base(Namespace.connection_timeout) {} 
		 public connection_timeout(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(host_gone))]
    public class host_gone : Tag
	{
		 public host_gone() : base(Namespace.host_gone) {} 
		 public host_gone(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(host_unknown))]
    public class host_unknown : Tag
	{
		 public host_unknown() : base(Namespace.host_unknown) {} 
		 public host_unknown(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(improper_addressing))]
    public class improper_addressing : Tag
	{
		 public improper_addressing() : base(Namespace.improper_addressing) {} 
		 public improper_addressing(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(internal_server_error))]
    public class internal_server_error : Tag
	{
		 public internal_server_error() : base(Namespace.internal_server_error) {} 
		 public internal_server_error(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(invalid_from))]
    public class invalid_from : Tag
	{
		 public invalid_from() : base(Namespace.invalid_from) {} 
		 public invalid_from(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(invalid_id))]
    public class invalid_id : Tag
	{
		 public invalid_id() : base(Namespace.invalid_id) {} 
		 public invalid_id(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(invalid_namespace))]
    public class invalid_namespace : Tag
	{
		 public invalid_namespace() : base(Namespace.invalid_namespace) {} 
		 public invalid_namespace(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(invalid_xml))]
    public class invalid_xml : Tag
	{
		 public invalid_xml() : base(Namespace.invalid_xml) {} 
		 public invalid_xml(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(not_authorized))]
    public class not_authorized : Tag
	{
		 public not_authorized() : base(Namespace.not_authorized) {} 
		 public not_authorized(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(policy_violation))]
    public class policy_violation : Tag
	{
		 public policy_violation() : base(Namespace.policy_violation) {} 
		 public policy_violation(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(remote_connection_failed))]
    public class remote_connection_failed : Tag
	{
		 public remote_connection_failed() : base(Namespace.remote_connection_failed) {} 
		 public remote_connection_failed(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(reset))]
    public class reset : Tag
	{
		 public reset() : base(Namespace.reset) {} 
		 public reset(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(resource_constraint))]
    public class resource_constraint : Tag
	{
		 public resource_constraint() : base(Namespace.resource_constraint) {} 
		 public resource_constraint(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(restricted_xml))]
    public class restricted_xml : Tag
	{
		 public restricted_xml() : base(Namespace.restricted_xml) {} 
		 public restricted_xml(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(see_other_host))]
    public class see_other_host : Tag
	{
		 public see_other_host() : base(Namespace.see_other_host) {} 
		 public see_other_host(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(system_shutdown))]
    public class system_shutdown : Tag
	{
		 public system_shutdown() : base(Namespace.system_shutdown) {} 
		 public system_shutdown(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(undefined_condition))]
    public class undefined_condition : Tag
	{
		 public undefined_condition() : base(Namespace.undefined_condition) {} 
		 public undefined_condition(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(unsupported_encoding))]
    public class unsupported_encoding : Tag
	{
		 public unsupported_encoding() : base(Namespace.unsupported_encoding) {} 
		 public unsupported_encoding(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(unsupported_stanza_type))]
    public class unsupported_stanza_type : Tag
	{
		 public unsupported_stanza_type() : base(Namespace.unsupported_stanza_type) {} 
		 public unsupported_stanza_type(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(unsupported_version))]
    public class unsupported_version : Tag
	{
		 public unsupported_version() : base(Namespace.unsupported_version) {} 
		 public unsupported_version(XElement other) : base(other) {} 
	}

    [XMPPTag(typeof(Namespace), typeof(xml_not_well_formed))]
    public class xml_not_well_formed : Tag
	{
		 public xml_not_well_formed() : base(Namespace.xml_not_well_formed) {}
         public xml_not_well_formed(XElement other) : base(other) { } 
	}

    [XMPPTag(typeof(Namespace), typeof(text))]
    public class text : Tag
    {
        public text() : base(Namespace.text) { }
        public text(XElement other) : base(other) { }
    }
}

/*

<?xml version="1.0" encoding="UTF-8" ?> 
- <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" targetNamespace="urn:ietf:params:xml:ns:xmpp-streams" xmlns="urn:ietf:params:xml:ns:xmpp-streams" elementFormDefault="qualified">
  <xs:import namespace="http://www.w3.org/XML/1998/namespace" schemaLocation="http://www.w3.org/2001/03/xml.xsd" /> 
  <xs:element name="bad-format" type="empty" /> 
  <xs:element name="bad-namespace-prefix" type="empty" /> 
  <xs:element name="conflict" type="empty" /> 
  <xs:element name="connection-timeout" type="empty" /> 
  <xs:element name="host-gone" type="empty" /> 
  <xs:element name="host-unknown" type="empty" /> 
  <xs:element name="improper-addressing" type="empty" /> 
  <xs:element name="internal-server-error" type="empty" /> 
  <xs:element name="invalid-from" type="empty" /> 
  <xs:element name="invalid-id" type="empty" /> 
  <xs:element name="invalid-namespace" type="empty" /> 
  <xs:element name="invalid-xml" type="empty" /> 
  <xs:element name="not-authorized" type="empty" /> 
  <xs:element name="policy-violation" type="empty" /> 
  <xs:element name="remote-connection-failed" type="empty" /> 
  <xs:element name="reset" type="empty" /> 
  <xs:element name="resource-constraint" type="empty" /> 
  <xs:element name="restricted-xml" type="empty" /> 
  <xs:element name="see-other-host" type="xs:string" /> 
  <xs:element name="system-shutdown" type="empty" /> 
  <xs:element name="undefined-condition" type="empty" /> 
  <xs:element name="unsupported-encoding" type="empty" /> 
  <xs:element name="unsupported-stanza-type" type="empty" /> 
  <xs:element name="unsupported-version" type="empty" /> 
  <xs:element name="xml-not-well-formed" type="empty" /> 
- <xs:group name="streamErrorGroup">
- <xs:choice>
  <xs:element ref="bad-format" /> 
  <xs:element ref="bad-namespace-prefix" /> 
  <xs:element ref="conflict" /> 
  <xs:element ref="connection-timeout" /> 
  <xs:element ref="host-gone" /> 
  <xs:element ref="host-unknown" /> 
  <xs:element ref="improper-addressing" /> 
  <xs:element ref="internal-server-error" /> 
  <xs:element ref="invalid-from" /> 
  <xs:element ref="invalid-id" /> 
  <xs:element ref="invalid-namespace" /> 
  <xs:element ref="invalid-xml" /> 
  <xs:element ref="not-authorized" /> 
  <xs:element ref="policy-violation" /> 
  <xs:element ref="remote-connection-failed" /> 
  <xs:element ref="reset" /> 
  <xs:element ref="resource-constraint" /> 
  <xs:element ref="restricted-xml" /> 
  <xs:element ref="see-other-host" /> 
  <xs:element ref="system-shutdown" /> 
  <xs:element ref="undefined-condition" /> 
  <xs:element ref="unsupported-encoding" /> 
  <xs:element ref="unsupported-stanza-type" /> 
  <xs:element ref="unsupported-version" /> 
  <xs:element ref="xml-not-well-formed" /> 
  </xs:choice>
  </xs:group>
- <xs:element name="text">
- <xs:complexType>
- <xs:simpleContent>
- <xs:extension base="xs:string">
  <xs:attribute ref="xml:lang" use="optional" /> 
  </xs:extension>
  </xs:simpleContent>
  </xs:complexType>
  </xs:element>
- <xs:simpleType name="empty">
- <xs:restriction base="xs:string">
  <xs:enumeration value="" /> 
  </xs:restriction>
  </xs:simpleType>
  </xs:schema>

*/