// xmpp_stanzas.cs
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

namespace XMPP.tags.xmpp_stanzas
{
    public class Namespace
    {
        public static string Name = "urn:ietf:params:xml:ns:xmpp-stanzas";
        public static XName bad_request = XName.Get("bad-request", Name);
        public static XName conflict = XName.Get("conflict", Name);
        public static XName feature_not_implemented = XName.Get("feature-not-implemented", Name);
        public static XName forbidden = XName.Get("forbidden", Name);
        public static XName gone = XName.Get("gone", Name);
        public static XName internal_server_error = XName.Get("internal-server-error", Name);
        public static XName item_not_found = XName.Get("item-not-found", Name);
        public static XName jid_malformed = XName.Get("jid-malformed", Name);
        public static XName not_acceptable = XName.Get("not-acceptable", Name);
        public static XName not_authorized = XName.Get("not-authorized", Name);
        public static XName not_allowed = XName.Get("not-allowed", Name);
        public static XName payment_required = XName.Get("payment-required", Name);
        public static XName policy_violation = XName.Get("policy-violation", Name);
        public static XName recipient_unavailable = XName.Get("recipient-unavailable", Name);
        public static XName redirect = XName.Get("redirect", Name);
        public static XName registration_required = XName.Get("registration-required", Name);
        public static XName remote_server_not_found = XName.Get("remote-server-not-found", Name);
        public static XName remote_server_timeout = XName.Get("remote-server-timeout", Name);
        public static XName resource_constraint = XName.Get("resource-constraint", Name);
        public static XName service_unavailable = XName.Get("service-unavailable", Name);
        public static XName subscription_required = XName.Get("subscription-required", Name);
        public static XName undefined_condition = XName.Get("undefined-condition", Name);
        public static XName unexpected_request = XName.Get("unexpected-request", Name);

        public static XName text = XName.Get("text", Name);

    }


    [XMPPTag(typeof(Namespace), typeof(bad_request))]
    public class bad_request : Tag
	{
		 public bad_request() : base(Namespace.bad_request) {} 
		 public bad_request(XElement other) : base(other) {} 
	}


    [XMPPTag(typeof(Namespace), typeof(conflict))]
    public class conflict : Tag 
	{
		public conflict() : base(Namespace.conflict) {} 
		public conflict(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(feature_not_implemented))]
    public class feature_not_implemented : Tag 
	{
		public feature_not_implemented() : base(Namespace.feature_not_implemented) {} 
		public feature_not_implemented(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(forbidden))]
    public class forbidden : Tag 
	{
		public forbidden() : base(Namespace.forbidden) {} 
		public forbidden(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(gone))]
    public class gone : Tag 
	{
		public gone() : base(Namespace.gone) {} 
		public gone(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(internal_server_error))]
    public class internal_server_error : Tag 
	{
		public internal_server_error() : base(Namespace.internal_server_error) {} 
		public internal_server_error(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(item_not_found))]
    public class item_not_found : Tag 
	{
		public item_not_found() : base(Namespace.item_not_found) {} 
		public item_not_found(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(jid_malformed))]
    public class jid_malformed : Tag 
	{
		public jid_malformed() : base(Namespace.jid_malformed) {} 
		public jid_malformed(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(not_acceptable))]
    public class not_acceptable : Tag 
	{
		public not_acceptable() : base(Namespace.not_acceptable) {} 
		public not_acceptable(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(not_authorized))]
    public class not_authorized : Tag 
	{
		public not_authorized() : base(Namespace.not_authorized) {} 
		public not_authorized(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(not_allowed))]
    public class not_allowed : Tag 
	{
		public not_allowed() : base(Namespace.not_allowed) {} 
		public not_allowed(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(payment_required))]
    public class payment_required : Tag 
	{
		public payment_required() : base(Namespace.payment_required) {} 
		public payment_required(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(policy_violation))]
    public class policy_violation : Tag 
	{
		public policy_violation() : base(Namespace.policy_violation) {} 
		public policy_violation(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(recipient_unavailable))]
    public class recipient_unavailable : Tag 
	{
		public recipient_unavailable() : base(Namespace.recipient_unavailable) {} 
		public recipient_unavailable(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(redirect))]
    public class redirect : Tag 
	{
		public redirect() : base(Namespace.redirect) {} 
		public redirect(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(registration_required))]
    public class registration_required : Tag 
	{
		public registration_required() : base(Namespace.registration_required) {} 
		public registration_required(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(remote_server_not_found))]
    public class remote_server_not_found : Tag 
	{
		public remote_server_not_found() : base(Namespace.remote_server_not_found) {} 
		public remote_server_not_found(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(remote_server_timeout))]
    public class remote_server_timeout : Tag 
	{
		public remote_server_timeout() : base(Namespace.remote_server_timeout) {} 
		public remote_server_timeout(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(resource_constraint))]
    public class resource_constraint : Tag 
	{
		public resource_constraint() : base(Namespace.resource_constraint) {} 
		public resource_constraint(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(service_unavailable))]
    public class service_unavailable : Tag 
	{
		public service_unavailable() : base(Namespace.service_unavailable) {} 
		public service_unavailable(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(subscription_required))]
    public class subscription_required : Tag 
	{
		public subscription_required() : base(Namespace.subscription_required) {} 
		public subscription_required(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(undefined_condition))]
    public class undefined_condition : Tag 
	{
		public undefined_condition() : base(Namespace.undefined_condition) {} 
		public undefined_condition(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(unexpected_request))]
    public class unexpected_request : Tag 
	{
		public unexpected_request() : base(Namespace.unexpected_request) {} 
		public unexpected_request(XElement other) : base(other) {}
	}


    [XMPPTag(typeof(Namespace), typeof(text))]
    public class text : Tag
    {
        public text() : base(Namespace.text) {} 
        public text(XElement other) : base(other) {}

        public string lang { get { return (string)GetAttributeValue(XName.Get("lang", xml.Namespace.Name)); } set { SetAttributeValue(XName.Get("lang", xml.Namespace.Name), value); } }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:ietf:params:xml:ns:xmpp-stanzas'
    xmlns='urn:ietf:params:xml:ns:xmpp-stanzas'
    elementFormDefault='qualified'>

  <xs:import namespace='http://www.w3.org/XML/1998/namespace'
             schemaLocation='http://www.w3.org/2001/03/xml.xsd'/>

  <xs:element name='bad-request' type='empty'/>
  <xs:element name='conflict' type='empty'/>
  <xs:element name='feature-not-implemented' type='empty'/>
  <xs:element name='forbidden' type='empty'/>
  <xs:element name='gone' type='xs:string'/>
  <xs:element name='internal-server-error' type='empty'/>
  <xs:element name='item-not-found' type='empty'/>
  <xs:element name='jid-malformed' type='empty'/>
  <xs:element name='not-acceptable' type='empty'/>
  <xs:element name='not-allowed' type='empty'/>
  <xs:element name='not-authorized' type='empty'/>
  <xs:element name='payment-required' type='empty'/>
  <xs:element name='policy-violation' type='empty'/>
  <xs:element name='recipient-unavailable' type='empty'/>
  <xs:element name='redirect' type='xs:string'/>
  <xs:element name='registration-required' type='empty'/>
  <xs:element name='remote-server-not-found' type='empty'/>
  <xs:element name='remote-server-timeout' type='empty'/>
  <xs:element name='resource-constraint' type='empty'/>
  <xs:element name='service-unavailable' type='empty'/>
  <xs:element name='subscription-required' type='empty'/>
  <xs:element name='undefined-condition' type='empty'/>
  <xs:element name='unexpected-request' type='empty'/>

  <xs:group name='stanzaErrorGroup'>
    <xs:choice>
      <xs:element ref='bad-request'/>
      <xs:element ref='conflict'/>
      <xs:element ref='feature-not-implemented'/>
      <xs:element ref='forbidden'/>
      <xs:element ref='gone'/>
      <xs:element ref='internal-server-error'/>
      <xs:element ref='item-not-found'/>
      <xs:element ref='jid-malformed'/>
      <xs:element ref='not-acceptable'/>
      <xs:element ref='not-authorized'/>
      <xs:element ref='not-allowed'/>
      <xs:element ref='payment-required'/>
      <xs:element ref='policy-violation'/>
      <xs:element ref='recipient-unavailable'/>
      <xs:element ref='redirect'/>
      <xs:element ref='registration-required'/>
      <xs:element ref='remote-server-not-found'/>
      <xs:element ref='remote-server-timeout'/>
      <xs:element ref='resource-constraint'/>
      <xs:element ref='service-unavailable'/>
      <xs:element ref='subscription-required'/>
      <xs:element ref='undefined-condition'/>
      <xs:element ref='unexpected-request'/>
    </xs:choice>
  </xs:group>

  <xs:element name='text'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='xs:string'>
          <xs:attribute ref='xml:lang' use='optional'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/