// xhtml_im.cs
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


namespace XMPP.tags.jabber.protocol.xhtml_im
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/xhtml-im";
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    xmlns:xhtml='http://www.w3.org/1999/xhtml'
    targetNamespace='http://jabber.org/protocol/xhtml-im'
    xmlns='http://jabber.org/protocol/xhtml-im'
    elementFormDefault='qualified'>

  <xs:import namespace='http://www.w3.org/1999/xhtml'
             schemaLocation='http://www.w3.org/2002/08/xhtml/xhtml1-strict.xsd'/>

  <xs:annotation>
    <xs:documentation>

      This schema defines the <html/> element qualified by
      the 'http://jabber.org/protocol/xhtml-im' namespace.
      The only allowable child is a <body/> element qualified
      by the 'http://www.w3.org/1999/xhtml' namespace. Refer 
      to the XHTML-IM schema driver for the definition of the 
      XHTML 1.0 Integration Set.

      Full documentation of this Integration Set is contained in
      "XEP-0071: XHTML-IM", a specification published by the
      XMPP Standards Foundation.

         http://www.xmpp.org/extensions/xep-0071.html

    </xs:documentation>
    <xs:documentation source="http://www.xmpp.org/extensions/xep-0071.html"/>
  </xs:annotation>

  <xs:element name='html'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='xhtml:body' 
                    minOccurs='0' 
                    maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

 * 
 * 
 *

<?xml version='1.0' encoding='UTF-8'?>

<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           targetNamespace="http://www.w3.org/1999/xhtml"
           xmlns="http://www.w3.org/1999/xhtml">

  <xs:annotation>
    <xs:documentation>

      This is the XML Schema module of named XHTML 1.0 content models 
      for XHTML-IM, an XHTML 1.0 Integration Set for use in exchanging 
      marked-up instant messages between entities that conform to 
      the Extensible Messaging and Presence Protocol (XMPP). This 
      Integration Set includes a subset of the modules defined for 
      XHTML 1.0 but does not redefine any existing modules, nor 
      does it define any new modules. Specifically, it includes the 
      following modules only:

         - Structure
         - Text
         - Hypertext
         - List
         - Image
         - Style Attribute
     
      Therefore XHTML-IM uses the following content models:

          Block.mix;            Block-like elements, e.g., paragraphs
          Flow.mix;             Any block or inline elements
          Inline.mix;           Character-level elements
          InlineNoAnchor.class; Anchor element
          InlinePre.mix;        Pre element

      XHTML-IM also uses the following Attribute Groups:

           Core.extra.attrib
           I18n.extra.attrib
           Common.extra

      Full documentation of this Integration Set is contained in
      "XEP-0071: XHTML-IM", a specification published by the
      XMPP Standards Foundation.

         http://www.xmpp.org/extensions/xep-0071.html

    </xs:documentation>
    <xs:documentation source="http://www.xmpp.org/extensions/xep-0071.html"/>
  </xs:annotation>

  <!-- BEGIN ATTRIBUTE GROUPS -->

  <xs:attributeGroup name="Core.extra.attrib"/>
  <xs:attributeGroup name="I18n.extra.attrib"/>
  <xs:attributeGroup name="Common.extra">
    <xs:attributeGroup ref="style.attrib"/>
  </xs:attributeGroup>

  <!-- END ATTRIBUTE GROUPS -->

  <!-- BEGIN HYPERTEXT MODULE "PRIMITIVES" -->

  <xs:group name="Anchor.class">
    <xs:sequence>
      <xs:element ref="a"/>
    </xs:sequence>
  </xs:group>

  <!-- END HYPERTEXT MODULE "PRIMITIVES" -->

  <!-- BEGIN IMAGE MODULE "PRIMITIVES" -->

  <xs:group name="Image.class">
    <xs:choice>
      <xs:element ref="img"/>
    </xs:choice>
  </xs:group>

  <!-- END IMAGE MODULE "PRIMITIVES" -->

  <!-- BEGIN LIST MODULE "PRIMITIVES" -->

  <xs:group name="List.class">
    <xs:choice>
      <xs:element ref="ul"/>
      <xs:element ref="ol"/>
      <xs:element ref="dl"/>
    </xs:choice>
  </xs:group>

  <!-- END LIST MODULE "PRIMITIVES" -->

  <!-- BEGIN TEXT MODULE "PRIMITIVES" -->

  <xs:group name="BlkPhras.class">
    <xs:choice>
      <xs:element ref="address"/>
      <xs:element ref="blockquote"/>
      <xs:element ref="pre"/>
    </xs:choice>
  </xs:group>

  <xs:group name="BlkStruct.class">
    <xs:choice>
      <xs:element ref="div"/>
      <xs:element ref="p"/>
    </xs:choice>
  </xs:group>

  <xs:group name="Heading.class">
    <xs:choice>
      <xs:element ref="h1"/>
      <xs:element ref="h2"/>
      <xs:element ref="h3"/>
      <xs:element ref="h4"/>
      <xs:element ref="h5"/>
      <xs:element ref="h6"/>
    </xs:choice>
  </xs:group>

  <xs:group name="InlPhras.class">
    <xs:choice>
      <xs:element ref="abbr"/>
      <xs:element ref="acronym"/>
      <xs:element ref="cite"/>
      <xs:element ref="code"/>
      <xs:element ref="dfn"/>
      <xs:element ref="em"/>
      <xs:element ref="kbd"/>
      <xs:element ref="q"/>
      <xs:element ref="samp"/>
      <xs:element ref="strong"/>
      <xs:element ref="var"/>
    </xs:choice>
  </xs:group>

  <xs:group name="InlStruct.class">
    <xs:choice>
      <xs:element ref="br"/>
      <xs:element ref="span"/>
    </xs:choice>
  </xs:group>

  <!-- END TEXT MODULE "PRIMITIVES" -->

  <!-- BEGIN BLOCK COMBINATIONS -->

  <xs:group name="Block.class">
    <xs:choice>
      <xs:group ref="BlkPhras.class"/>
      <xs:group ref="BlkStruct.class"/>
    </xs:choice>
  </xs:group>

  <!-- END BLOCK COMBINATIONS -->

  <!-- BEGIN INLINE COMBINATIONS -->

  <!-- Any inline content -->
  <xs:group name="Inline.class">
    <xs:choice>
      <xs:group ref="Anchor.class"/>
      <xs:group ref="Image.class"/>
      <xs:group ref="InlPhras.class"/>
      <xs:group ref="InlStruct.class"/>
    </xs:choice>
  </xs:group>

  <!-- Inline content contained in a hyperlink -->
  <xs:group name="InlNoAnchor.class">
    <xs:choice>
      <xs:group ref="Image.class"/>
      <xs:group ref="InlStruct.class"/>
      <xs:group ref="InlPhras.class"/>
    </xs:choice>
  </xs:group>

  <!-- END INLINE COMBINATIONS -->

  <!-- BEGIN TOP-LEVEL MIXES -->

  <xs:group name="Block.mix">
    <xs:choice>
      <xs:group ref="Block.class"/>
      <xs:group ref="Heading.class"/>
      <xs:group ref="List.class"/>
    </xs:choice>
  </xs:group>

  <xs:group name="Flow.mix">
    <xs:choice>
      <xs:group ref="Block.class"/>
      <xs:group ref="Heading.class"/>
      <xs:group ref="Inline.class"/>
      <xs:group ref="List.class"/>
    </xs:choice>
  </xs:group>

  <xs:group name="Inline.mix">
    <xs:choice>
      <xs:group ref="Inline.class"/>
    </xs:choice>
  </xs:group>

  <xs:group name="InlNoAnchor.mix">
    <xs:choice>
      <xs:group ref="InlNoAnchor.class"/>
    </xs:choice>
  </xs:group>

  <xs:group name="InlinePre.mix">
    <xs:choice>
      <xs:group ref="Anchor.class"/>
      <xs:group ref="InlPhras.class"/>
      <xs:group ref="InlStruct.class"/>
    </xs:choice>
  </xs:group>

  <!-- END TOP-LEVEL MIXES -->

</xs:schema>

*/