using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Xmpp
{
    /// <summary>
    /// Provides a factory method for creating XElement instances and adds
    /// a couple of useful shortcut extensions to the XElement class.
    /// </summary>
    internal static class Xml
    {
        /// <summary>
        /// Creates a new XElement instance.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="namespace">The namespace of the element.</param>
        /// <returns>An initialized instance of the XElement class.</returns>
        /// <exception cref="ArgumentNullException">The name parameter is null.</exception>
        /// <exception cref="ArgumentException">The name parameter is the
        /// empty string.</exception>
        /// <exception cref="XmlException">The name or the namespace parameter
        /// is invalid.</exception>
        public static XElement Element(string name, string @namespace = null)
        {
            name.ThrowIfNullOrEmpty("name");
            return new XDocument().Element(XName.Get(name, @namespace));
        }

        /// <summary>
        /// Adds the specified element to the end of the list of child nodes, of
        /// this node.
        /// </summary>
        /// <param name="e">The XElement instance the method is invoked for.</param>
        /// <param name="child">The node to add.</param>
        /// <returns>A reference to the XElement instance.</returns>
        public static XElement Child(this XElement e, XElement child)
        {
            e.Add(child);   
            return e;
        }

        /// <summary>
        /// Sets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="e">The XElement instance the method is invoked for.</param>
        /// <param name="name">The name of the attribute to create or alter.</param>
        /// <param name="value">The value to set for the attribute.</param>
        /// <returns>A reference to the XElement instance.</returns>
        public static XElement SetAttribute(this XElement e, string name, string value)
        {
            e.SetAttributeValue(XName.Get(name), value);
            return e;
        }

        /// <summary>
        /// Adds the specified text to the end of the list of child nodes, of
        /// this node.
        /// </summary>
        /// <param name="e">The XElement instance the method is invoked for.</param>
        /// <param name="text">The text to add.</param>
        /// <returns>A reference to the XElement instance.</returns>
        public static XElement Text(this XElement e, string text)
        {
            e.SetValue(text);
            return e;
        }

        public static XElement RemoveChild(this XElement e, XElement removedElement)
        {
            removedElement.Remove();
            return e;
        }

        public static void RemoveAttribute(this XElement e, string name)
        {
            var att = e.Attribute(XName.Get(name));
            att.Remove();
        }

        public static string GetAttribute(this XElement e, string name)
        {
            var att = e.Attribute(name);
            return att != null ? att.Value : null;
        }

        /// <summary>
        /// Serializes the XElement instance into a string.
        /// </summary>
        /// <param name="e">The XElement instance the method is invoked for.</param>
        /// <param name="xmlDeclaration">true to include a XML declaration,
        /// otherwise false.</param>
        /// <param name="leaveOpen">true to leave the tag of an empty element
        /// open, otherwise false.</param>
        /// <returns>A textual representation of the XElement instance.</returns>
        public static string ToXmlString(this XElement e, bool xmlDeclaration = false,
            bool leaveOpen = false)
        {
            // Can't use e.OuterXml because it "messes up" namespaces for elements with
            // a prefix, i.e. stream:stream (What it does is probably correct, but just
            // not what we need for XMPP).
            StringBuilder b = new StringBuilder("<" + e.Name);
            if (!String.IsNullOrEmpty(e.GetDefaultNamespace().NamespaceName))
                b.Append(" xmlns='" + e.GetDefaultNamespace().NamespaceName + "'");
            foreach (XAttribute a in e.Attributes())
            {
                if (a.Name == "xmlns")
                    continue;
                if (a.Value != null)
                    b.Append(" " + a.Name + "='" + a.Value.ToString()
                        + "'");
            }
            if (e.IsEmpty)
                b.Append("/>");
            else
            {
                b.Append(">");
                foreach (var child in e.Nodes())
                {
                    if (child is XElement)
                        b.Append(((XElement)child).ToXmlString());
                    else if (child is XText)
                        b.Append(((XText)child).Value);
                }
                b.Append("</" + e.Name + ">");
            }
            string xml = b.ToString();
            if (xmlDeclaration)
                xml = "<?xml version='1.0' encoding='UTF-8'?>" + xml;
            if (leaveOpen)
                return Regex.Replace(xml, "/>$", ">");
            return xml;
        }
    }
}