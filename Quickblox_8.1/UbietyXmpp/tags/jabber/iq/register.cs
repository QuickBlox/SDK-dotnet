// register.cs
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

namespace XMPP.tags.jabber.iq.register
{
    public class Namespace
    {
        public static string Name = "jabber:iq:register";
        public static XName query = XName.Get("query", Name);

        public static XName registered = XName.Get("registered", Name);
        public static XName instruction = XName.Get("instruction", Name);
        public static XName username = XName.Get("username", Name);
        public static XName nick = XName.Get("nick", Name);
        public static XName password = XName.Get("password", Name);
        public static XName name = XName.Get("name", Name);
        public static XName first = XName.Get("first", Name);
        public static XName last = XName.Get("last", Name);
        public static XName email = XName.Get("email", Name);
        public static XName address = XName.Get("address", Name);
        public static XName city = XName.Get("city", Name);
        public static XName state = XName.Get("state", Name);
        public static XName zip = XName.Get("zip", Name);
        public static XName phone = XName.Get("phone", Name);
        public static XName url = XName.Get("url", Name);
        public static XName date = XName.Get("date", Name);
        public static XName misc = XName.Get("misc", Name);
        public static XName text = XName.Get("text", Name);
        public static XName key = XName.Get("key", Name);
        public static XName remove = XName.Get("remove", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(query))]
    public class query : Tag
    {
        public query() : base(Namespace.query) {} 
        public query(XElement other) : base(other) {} 

        public IEnumerable<registered>    registeredElements { get { return Elements<registered>(Namespace.registered); } }
        public IEnumerable<instruction>    instructionElements { get { return Elements<instruction>(Namespace.instruction); } }
        public IEnumerable<username>    usernameElements { get { return Elements<username>(Namespace.username); } }
        public IEnumerable<nick>    nickElements { get { return Elements<nick>(Namespace.nick); } }
        public IEnumerable<password>    passwordElements { get { return Elements<password>(Namespace.password); } }
        public IEnumerable<name>    nameElements { get { return Elements<name>(Namespace.name); } }
        public IEnumerable<first>    firstElements { get { return Elements<first>(Namespace.first); } }
        public IEnumerable<last>    lastElements { get { return Elements<last>(Namespace.last); } }
        public IEnumerable<email>    emailElements { get { return Elements<email>(Namespace.email); } }
        public IEnumerable<address>    addressElements { get { return Elements<address>(Namespace.address); } }
        public IEnumerable<city>    cityElements { get { return Elements<city>(Namespace.city); } }
        public IEnumerable<state>    stateElements { get { return Elements<state>(Namespace.state); } }
        public IEnumerable<zip>    zipElements { get { return Elements<zip>(Namespace.zip); } }
        public IEnumerable<phone>    phoneElements { get { return Elements<phone>(Namespace.phone); } }
        public IEnumerable<url>    urlElements { get { return Elements<url>(Namespace.url); } }
        public IEnumerable<date>    dateElements { get { return Elements<date>(Namespace.date); } }
        public IEnumerable<misc>    miscElements { get { return Elements<misc>(Namespace.misc); } }
        public IEnumerable<text>    textElements { get { return Elements<text>(Namespace.text); } }
        public IEnumerable<key>    keyElements { get { return Elements<key>(Namespace.key); } }
        public IEnumerable<remove>    removeElements { get { return Elements<remove>(Namespace.remove); } }
    }

    [XMPPTag(typeof(Namespace), typeof(registered))]
    public class registered : Tag
    { 
        public registered() : base(Namespace.registered) {} 
        public registered(XElement other) : base(other) {} 
    }

    [XMPPTag(typeof(Namespace), typeof(instruction))]
    public class instruction : Tag
    {
        public instruction() : base(Namespace.instruction) { }
        public instruction(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(username))]
    public class username : Tag
    {
        public username() : base(Namespace.username) { }
        public username(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(nick))]
    public class nick : Tag
    {
        public nick() : base(Namespace.nick) { }
        public nick(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(password))]
    public class password : Tag
    {
        public password() : base(Namespace.password) { }
        public password(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(name))]
    public class name : Tag
    {
        public name() : base(Namespace.name) { }
        public name(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(first))]
    public class first : Tag
    {
        public first() : base(Namespace.first) { }
        public first(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(last))]
    public class last : Tag
    {
        public last() : base(Namespace.last) { }
        public last(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(email))]
    public class email : Tag
    {
        public email() : base(Namespace.email) { }
        public email(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(address))]
    public class address : Tag
    {
        public address() : base(Namespace.address) { }
        public address(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(city))]
    public class city : Tag
    {
        public city() : base(Namespace.city) { }
        public city(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(state))]
    public class state : Tag
    {
        public state() : base(Namespace.state) { }
        public state(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(zip))]
    public class zip : Tag
    {
        public zip() : base(Namespace.zip) { }
        public zip(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(phone))]
    public class phone : Tag
    {
        public phone() : base(Namespace.phone) { }
        public phone(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(url))]
    public class url : Tag
    {
        public url() : base(Namespace.url) { }
        public url(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(date))]
    public class date : Tag
    {
        public date() : base(Namespace.date) { }
        public date(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(misc))]
    public class misc : Tag
    {
        public misc() : base(Namespace.misc) { }
        public misc(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(text))]
    public class text : Tag
    {
        public text() : base(Namespace.text) { }
        public text(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(key))]
    public class key : Tag
    {
        public key() : base(Namespace.key) { }
        public key(XElement other) : base(other) { }
    }

    [XMPPTag(typeof(Namespace), typeof(remove))]
    public class remove : Tag
    {
        public remove() : base(Namespace.remove) { }
        public remove(XElement other) : base(other) { }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='jabber:iq:register'
    xmlns='jabber:iq:register'
    elementFormDefault='qualified'>

  <xs:import namespace='jabber:x:data'
             schemaLocation='http://xmpp.org/schemas/x-data.xsd'/>
  <xs:import namespace='jabber:x:oob'
             schemaLocation='http://xmpp.org/schemas/x-oob.xsd'/>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0077: http://www.xmpp.org/extensions/xep-0077.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='query'>
    <xs:complexType>
      <xs:sequence xmlns:xdata='jabber:x:data'
                   xmlns:xoob='jabber:x:oob'>
        <xs:choice minOccurs='0'>
          <xs:sequence minOccurs='0'>
            <xs:element name='registered' type='empty' minOccurs='0'/>
            <xs:element name='instructions' type='xs:string' minOccurs='0'/>
            <xs:element name='username' type='xs:string' minOccurs='0'/>
            <xs:element name='nick' type='xs:string' minOccurs='0'/>
            <xs:element name='password' type='xs:string' minOccurs='0'/>
            <xs:element name='name' type='xs:string' minOccurs='0'/>
            <xs:element name='first' type='xs:string' minOccurs='0'/>
            <xs:element name='last' type='xs:string' minOccurs='0'/>
            <xs:element name='email' type='xs:string' minOccurs='0'/>
            <xs:element name='address' type='xs:string' minOccurs='0'/>
            <xs:element name='city' type='xs:string' minOccurs='0'/>
            <xs:element name='state' type='xs:string' minOccurs='0'/>
            <xs:element name='zip' type='xs:string' minOccurs='0'/>
            <xs:element name='phone' type='xs:string' minOccurs='0'/>
            <xs:element name='url' type='xs:string' minOccurs='0'/>
            <xs:element name='date' type='xs:string' minOccurs='0'/>
            <xs:element name='misc' type='xs:string' minOccurs='0'/>
            <xs:element name='text' type='xs:string' minOccurs='0'/>
            <xs:element name='key' type='xs:string' minOccurs='0'/>
          </xs:sequence>
          <xs:element name='remove' type='empty' minOccurs='0'/>
        </xs:choice>
        <xs:element ref='xdata:x' minOccurs='0'/>
        <xs:element ref='xoob:x' minOccurs='0'/>
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