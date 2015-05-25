// xmpp_sasl.cs
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

using XMPP.registries;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Linq;
using System.Linq;

namespace XMPP.tags.xmpp_sasl
{
    public class Namespace
    {
        public static string Name = "urn:ietf:params:xml:ns:xmpp-sasl";
        public static XName mechanisms = XName.Get("mechanisms", Name);
        public static XName mechanism = XName.Get("mechanism", Name);
        public static XName abort = XName.Get("abort", Name);
        public static XName auth = XName.Get("auth", Name);
        public static XName challenge = XName.Get("challenge", Name);
        public static XName response = XName.Get("response", Name);
        public static XName success = XName.Get("success", Name);
        public static XName failure = XName.Get("failure", Name);
        public static XName text = XName.Get("text", Name);

        public static XName bad_protocol = XName.Get("bad-protocol", Name);
        public static XName malformed_request = XName.Get("malformed-request", Name);

        public static XName aborted = XName.Get("aborted", Name);
        public static XName account_disabled = XName.Get("account-disabled", Name);
        public static XName credentials_expired = XName.Get("credentials-expired", Name);
        public static XName encryption_required = XName.Get("encryption-required", Name);
        public static XName incorrect_encoding = XName.Get("incorrect-encoding", Name);
        public static XName invalid_authzid = XName.Get("invalid-authzid", Name);
        public static XName invalid_mechanism = XName.Get("invalid-mechanism", Name);
        public static XName mechanism_too_weak = XName.Get("mechanism-too-weak", Name);
        public static XName not_authorized = XName.Get("not-authorized", Name);
        public static XName temporary_auth_failure = XName.Get("temporary-auth-failure", Name);
        public static XName transition_needed = XName.Get("transition-needed", Name);

        public static XName required = XName.Get("required", Name);
        public static XName optional = XName.Get("optional", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(mechanisms))]
    public class mechanisms : Tag
    {
        public mechanisms() : base(Namespace.mechanisms) { }
        public mechanisms(XElement other) : base(other) { }

        public MechanismType Types
        {
            get
            {
                return mechanismElements.Aggregate(MechanismType.None, (current, m) => current | m.Type);
            }
        }

        public IEnumerable<mechanism> mechanismElements { get { return Elements<mechanism>(Namespace.mechanism); } }
    }


    [XMPPTag(typeof(Namespace), typeof(mechanism))]
    public class mechanism : Tag
    {
        public mechanism() : base(Namespace.mechanism) { }
        public mechanism(XElement other) : base(other) { }

        public MechanismType Type { get { return ToType(Value); } set { Value = ToString(value); } }

        public static MechanismType ToType(string type)
        {
            switch (type)
            {
                case "PLAIN":
                    return MechanismType.Plain;
                case "DIGEST-MD5":
                    return MechanismType.DigestMD5;
                case "EXTERNAL":
                    return MechanismType.External;
                case "SCRAM-SHA-1":
                    return MechanismType.SCRAM;
                case "X-OAUTH2":
                    return MechanismType.XOAUTH2;
                default:
                    return MechanismType.None;
            }
        }

        public static string ToString(MechanismType type)
        {
            switch (type)
            {
                case MechanismType.Plain:
                    return "PLAIN";
                case MechanismType.External:
                    return "EXTERNAL";
                case MechanismType.DigestMD5:
                    return "DIGEST-MD5";
                case MechanismType.SCRAM:
                    return "SCRAM-SHA-1";
                case MechanismType.XOAUTH2:
                    return "X-OAUTH2";
                default:
                    return "";
            }
        }
    }


    [XMPPTag(typeof(Namespace), typeof(abort))]
    public class abort : Tag 
    {
        public abort() : base(Namespace.abort) { }
        public abort(XElement other) : base(other) { } 
    }


    [XMPPTag(typeof(Namespace), typeof(auth))]
    public class auth : Tag
    {
        public auth() : base(Namespace.auth) { }
        public auth(XElement other) : base(other) { }

        public MechanismType mechanism { get { return xmpp_sasl.mechanism.ToType((string)GetAttributeValue("mechanism")); } set { SetAttributeValue("mechanism", xmpp_sasl.mechanism.ToString(value)); } }
    }


    [XMPPTag(typeof(Namespace), typeof(challenge))]
    public class challenge : Tag
    {
        public challenge() : base(Namespace.challenge) { }
        public challenge(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(response))]
    public class response : Tag
    {
        public response() : base(Namespace.response) { }
        public response(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(success))]
    public class success : Tag
    {
        public success() : base(Namespace.success) { }
        public success(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(failure))]
    public class failure : Tag
    {
        public failure() : base(Namespace.failure) { }
        public failure(XElement other) : base(other) { }

        public enum typeEnum
        {
            none,
            aborted,
            account_disabled,
            credentials_expired,
            encryption_required,
            incorrect_encoding,
            invalid_authzid,
            invalid_mechanism,
            mechanism_too_weak,
            not_authorized,
            temporary_auth_failure, 
            transition_needed
        }

        public typeEnum types
        {
            get
            {
                typeEnum types = typeEnum.none;
                if (aborted != null) types = types | typeEnum.aborted;
                if (account_disabled != null) types = types | typeEnum.account_disabled;
                if (credentials_expired != null) types = types | typeEnum.credentials_expired;
                if (encryption_required != null) types = types | typeEnum.encryption_required;
                if (incorrect_encoding != null) types = types | typeEnum.incorrect_encoding;
                if (invalid_authzid != null) types = types | typeEnum.invalid_authzid;
                if (invalid_mechanism != null) types = types | typeEnum.invalid_mechanism;
                if (mechanism_too_weak != null) types = types | typeEnum.mechanism_too_weak;
                if (not_authorized != null) types = types | typeEnum.not_authorized;
                if (temporary_auth_failure != null) types = types | typeEnum.temporary_auth_failure;
                if (transition_needed != null) types = types | typeEnum.transition_needed;
                return types;
            }
        }

		public aborted aborted { get { return Element<aborted>(Namespace.aborted); } }
		public account_disabled account_disabled { get { return Element<account_disabled>(Namespace.account_disabled); } }
		public credentials_expired credentials_expired { get { return Element<credentials_expired>(Namespace.credentials_expired); } }
		public encryption_required encryption_required { get { return Element<encryption_required>(Namespace.encryption_required); } }
		public incorrect_encoding incorrect_encoding { get { return Element<incorrect_encoding>(Namespace.incorrect_encoding); } }
		public invalid_authzid invalid_authzid { get { return Element<invalid_authzid>(Namespace.invalid_authzid); } }
		public invalid_mechanism invalid_mechanism { get { return Element<invalid_mechanism>(Namespace.invalid_mechanism); } }
		public mechanism_too_weak mechanism_too_weak { get { return Element<mechanism_too_weak>(Namespace.mechanism_too_weak); } }
		public not_authorized not_authorized { get { return Element<not_authorized>(Namespace.not_authorized); } }
		public temporary_auth_failure temporary_auth_failure { get { return Element<temporary_auth_failure>(Namespace.temporary_auth_failure); } }
		public transition_needed transition_needed { get { return Element<transition_needed>(Namespace.transition_needed); } }
        public IEnumerable<text> textElements { get { return Elements<text>(Namespace.text); } }
    }


    [XMPPTag(typeof(Namespace), typeof(text))]
    public class text : Tag
    {
        public text() : base(Namespace.text) { }
        public text(XElement other) : base(other) { }

        public string lang { get { return (string)GetAttributeValue(XName.Get("lang", xml.Namespace.Name)); } set { SetAttributeValue(XName.Get("lang", xml.Namespace.Name), value); } }
    }


    [XMPPTag(typeof(Namespace), typeof(bad_protocol))]
    public class bad_protocol : Tag 
    {
        public bad_protocol() : base(Namespace.bad_protocol) { }
        public bad_protocol(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(malformed_request))]
    public class malformed_request : Tag 
    {
        public malformed_request() : base(Namespace.malformed_request) { }
        public malformed_request(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(aborted))]
    public class aborted : Tag 
    {
        public aborted() : base(Namespace.aborted) { }
        public aborted(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(account_disabled))]
    public class account_disabled : Tag 
    {
        public account_disabled() : base(Namespace.account_disabled) { }
        public account_disabled(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(credentials_expired))]
    public class credentials_expired : Tag 
    {
        public credentials_expired() : base(Namespace.credentials_expired) { }
        public credentials_expired(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(encryption_required))]
    public class encryption_required : Tag 
    {
        public encryption_required() : base(Namespace.encryption_required) { }
        public encryption_required(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(incorrect_encoding))]
    public class incorrect_encoding : Tag 
    {
        public incorrect_encoding() : base(Namespace.incorrect_encoding) { }
        public incorrect_encoding(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(invalid_authzid))]
    public class invalid_authzid : Tag 
    {
        public invalid_authzid() : base(Namespace.invalid_authzid) { }
        public invalid_authzid(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(invalid_mechanism))]
    public class invalid_mechanism : Tag 
    {
        public invalid_mechanism() : base(Namespace.invalid_mechanism) { }
        public invalid_mechanism(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(mechanism_too_weak))]
    public class mechanism_too_weak : Tag 
    {
        public mechanism_too_weak() : base(Namespace.mechanism_too_weak) { }
        public mechanism_too_weak(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(not_authorized))]
    public class not_authorized : Tag
    {
        public not_authorized() : base(Namespace.not_authorized) { }
        public not_authorized(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(temporary_auth_failure))]
    public class temporary_auth_failure : Tag
    {
        public temporary_auth_failure() : base(Namespace.temporary_auth_failure) { }
        public temporary_auth_failure(XElement other) : base(other) { }
    }


    [XMPPTag(typeof(Namespace), typeof(transition_needed))]
    public class transition_needed : Tag
    {
        public transition_needed() : base(Namespace.transition_needed) { }
        public transition_needed(XElement other) : base(other) { }
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
    targetNamespace='urn:ietf:params:xml:ns:xmpp-sasl'
    xmlns='urn:ietf:params:xml:ns:xmpp-sasl'
    elementFormDefault='qualified'>

  <xs:import namespace='http://www.w3.org/XML/1998/namespace'
             schemaLocation='http://www.w3.org/2001/03/xml.xsd'/>

  <xs:element name='mechanisms'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='mechanism'
                    minOccurs='1'
                    maxOccurs='unbounded'
                    type='xs:NMTOKEN'/>
        <xs:any namespace='##other'
                minOccurs='0'
                maxOccurs='unbounded'
                processContents='lax'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='abort' type='empty'/>

  <xs:element name='auth'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='xs:string'>
          <xs:attribute name='mechanism'
                        type='xs:NMTOKEN'
                        use='required'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='challenge' type='xs:string'/>

  <xs:element name='response' type='xs:string'/>

  <xs:element name='success' type='xs:string'/>

  <xs:element name='failure'>
    <xs:complexType>
      <xs:sequence>
        <xs:choice minOccurs='0'>
          <xs:element name='aborted' type='empty'/>
          <xs:element name='account-disabled' type='empty'/>
          <xs:element name='credentials-expired' type='empty'/>
          <xs:element name='encryption-required' type='empty'/>
          <xs:element name='incorrect-encoding' type='empty'/>
          <xs:element name='invalid-authzid' type='empty'/>
          <xs:element name='invalid-mechanism' type='empty'/>
          <xs:element name='malformed-request' type='empty'/>
          <xs:element name='mechanism-too-weak' type='empty'/>
          <xs:element name='not-authorized' type='empty'/>
          <xs:element name='temporary-auth-failure' type='empty'/>
          <xs:element name='transition-needed' type='empty'/>
        </xs:choice>
        <xs:element ref='text' minOccurs='0' maxOccurs='1'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

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