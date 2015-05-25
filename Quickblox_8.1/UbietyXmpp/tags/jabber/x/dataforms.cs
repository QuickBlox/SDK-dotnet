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
using System.Runtime.Serialization;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.jabber.x.dataforms
{
    public class Namespace
    {
        public const string Name = "jabber:x:data";

        public static XName x = XName.Get("x", Name);
        public static XName title = XName.Get("title", Name);
        public static XName item = XName.Get("item", Name);
        public static XName reported = XName.Get("reported", Name);
        public static XName field = XName.Get("field", Name);
        public static XName desc = XName.Get("desc", Name);
        public static XName value = XName.Get("value", Name);
        public static XName option = XName.Get("option", Name);
        public static XName required = XName.Get("required", Name);
    }

    [XMPPTag(typeof(Namespace), typeof(x))]
    public class x : Tag
    {
        public enum typeEnum
        {
            cancel,
            form,
            result,
            submit
        }

        public x() : base(Namespace.x) { }
        public x(XElement other) : base(other) { }

        public IEnumerable<field> fieldElements
        {
            get { return Elements<field>(Namespace.field); }
        }

        public IEnumerable<item> itemElements
        {
            get { return Elements<item>(Namespace.item); }
        }

        public typeEnum type
        {
            get { return GetAttributeEnum<typeEnum>("type"); }
            set { SetAttributeEnum<typeEnum>("type", value); }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(title))]
    public class title : Tag
    {
        public title() : base(Namespace.title) { }
        public title(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(item))]
    public class item : Tag
    {
        public item() : base(Namespace.item) { }
        public item(XElement other) : base(other) { }

        public IEnumerable<field> fieldElements
        {
            get { return Elements<field>(Namespace.field); }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(reported))]
    public class reported : Tag
    {
        public reported() : base(Namespace.item) { }
        public reported(XElement other) : base(other) { }

        public IEnumerable<field> fieldElements
        {
            get { return Elements<field>(Namespace.field); }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(field))]
    public class field : Tag
    {
        public enum typeEnum
        {
            boolean,
            @fixed,
            hidden,
            [EnumMember(Value = "jid-multi")]
            jid_multi,
            [EnumMember(Value = "jid-single")]
            jid_single,
            [EnumMember(Value = "list-multi")]
            list_multi,
            [EnumMember(Value = "list-single")]
            list_single,
            [EnumMember(Value = "text-multi")]
            text_multi,
            [EnumMember(Value = "text-private")]
            text_private,
            [EnumMember(Value = "text-single")]
            text_single
        }

        public field() : base(Namespace.field) { }
        public field(XElement other) : base(other) { }

        public XElement desc
        {
            get { return Element<desc>(Namespace.desc); }
        }

        public XElement required
        {
            get { return Element<required>(Namespace.required); }
        }

        public IEnumerable<value> valueElements
        {
            get { return Elements<value>(Namespace.value); }
        }

        public IEnumerable<option> optionElements
        {
            get { return Elements<option>(Namespace.option); }
        }

        public string label
        {
            get { return (string)GetAttributeValue("label"); }
            set { SetAttributeValue("label", value); }
        }

        public typeEnum type
        {
            get { return GetAttributeEnum<typeEnum>("type"); }
            set { SetAttributeEnum<typeEnum>("type", value); }
        }

        public string var
        {
            get { return (string)GetAttributeValue("var"); }
            set { SetAttributeValue("var", value); }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(desc))]
    public class desc : Tag
    {
        public desc() : base(Namespace.desc) { }
        public desc(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(value))]
    public class value : Tag
    {
        public value() : base(Namespace.value) { }
        public value(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(option))]
    public class option : Tag
    {
        public option() : base(Namespace.option) { }
        public option(XElement other) : base(other) { }

        public string label
        {
            get { return (string)GetAttributeValue("label"); }
            set { SetAttributeValue("label", value); }
        }
    }

    [XMPPTag(typeof(Namespace), typeof(required))]
    public class required : Tag
    {
        public required() : base(Namespace.required) { }
        public required(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:x:data'
    xmlns='jabber:x:data'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0004: http://www.xmpp.org/extensions/xep-0004.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='x'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='instructions' 
                    minOccurs='0' 
                    maxOccurs='unbounded' 
                    type='xs:string'/>
        <xs:element name='title' minOccurs='0' type='xs:string'/>
        <xs:element ref='field' minOccurs='0' maxOccurs='unbounded'/>
        <xs:element ref='reported' minOccurs='0' maxOccurs='1'/>
        <xs:element ref='item' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='type' use='required'>
        <xs:simpleType>
          <xs:restriction base='xs:NCName'>
            <xs:enumeration value='cancel'/>
            <xs:enumeration value='form'/>
            <xs:enumeration value='result'/>
            <xs:enumeration value='submit'/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
    </xs:complexType>
  </xs:element>

  <xs:element name='field'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='desc' minOccurs='0' type='xs:string'/>
        <xs:element name='required' minOccurs='0' type='empty'/>
        <xs:element ref='value' minOccurs='0' maxOccurs='unbounded'/>
        <xs:element ref='option' minOccurs='0' maxOccurs='unbounded'/>
      </xs:sequence>
      <xs:attribute name='label' type='xs:string' use='optional'/>
      <xs:attribute name='type' use='optional' default='text-single'>
        <xs:simpleType>
          <xs:restriction base='xs:NCName'>
            <xs:enumeration value='boolean'/>
            <xs:enumeration value='fixed'/>
            <xs:enumeration value='hidden'/>
            <xs:enumeration value='jid-multi'/>
            <xs:enumeration value='jid-single'/>
            <xs:enumeration value='list-multi'/>
            <xs:enumeration value='list-single'/>
            <xs:enumeration value='text-multi'/>
            <xs:enumeration value='text-private'/>
            <xs:enumeration value='text-single'/>
          </xs:restriction>
        </xs:simpleType>
      </xs:attribute>
      <xs:attribute name='var' type='xs:string' use='optional'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='option'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='value'/>
      </xs:sequence>
      <xs:attribute name='label' type='xs:string' use='optional'/>
    </xs:complexType>
  </xs:element>

  <xs:element name='value' type='xs:string'/>

  <xs:element name='reported'>
    <xs:annotation>
      <xs:documentation>
        When contained in a "reported" element, the "field" element
        SHOULD NOT contain a "value" child.
      </xs:documentation>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='field' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='item'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='field' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>
*/
