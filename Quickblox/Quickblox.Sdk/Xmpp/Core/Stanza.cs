﻿using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Xmpp.Core
{
    /// <summary>
    /// Represents the base class for XML stanzas as are used by XMPP from which
    /// all implementations must derive.
    /// </summary>
    public abstract class Stanza
    {
        /// <summary>
        /// The XElement containing the actual data.
        /// </summary>
        protected XElement element;

        /// <summary>
        /// Specifies the JID of the intended recipient for the stanza.
        /// </summary>
        public Jid To
        {
            get
            {
                string v = element.GetAttribute("to");
                return String.IsNullOrEmpty(v) ? null : new Jid(v);
            }

            set
            {
                if (value == null)
                    element.RemoveAttribute("to");
                else
                    element.SetAttribute("to", value.ToString());
            }
        }

        /// <summary>
        /// Specifies the JID of the sender. If this is null, the stanza was generated
        /// by the client's server.
        /// </summary>
        public Jid From
        {
            get
            {
                string v = element.GetAttribute("from");
                return String.IsNullOrEmpty(v) ? null : new Jid(v);
            }

            set
            {
                if (value == null)
                    element.RemoveAttribute("from");
                else
                    element.SetAttribute("from", value.ToString());
            }
        }

        /// <summary>
        /// The ID of the stanza, which may be used for internal tracking of stanzas.
        /// </summary>
        public string Id
        {
            get
            {
                var v = element.GetAttribute("id");
                return String.IsNullOrEmpty(v) ? null : v;
            }

            set
            {
                if (value == null)
                    element.RemoveAttribute("id");
                else
                    element.SetAttribute("id", value);
            }
        }

        /// <summary>
        /// The language of the XML character data if the stanza contains data that is
        /// intended to be presented to a human user.
        /// </summary>
        public CultureInfo Language
        {
            get
            {
                var xname = XName.Get("lang", XNamespace.Xml.NamespaceName);
                string v = element.GetAttribute(xname);
                return String.IsNullOrEmpty(v) ? null : new CultureInfo(v);
            }

            set
            {

                if (value == null)
                    element.RemoveAttribute("lang", XNamespace.Xml.NamespaceName);
                else
                {
                    XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", value.Name);
                    this.element.Add(xmlLang);
                }
            }
        }

        /// <summary>
        /// The data of the stanza.
        /// </summary>
        public XElement Data
        {
            get
            {
                return element;
            }
        }

        /// <summary>
        /// Represent xml in string format
        /// </summary>
        public string DataString
        {
            get
            {
                return element.ToXmlString();
            }
        }

        /// <summary>
        /// Determines whether the stanza is empty, i.e. has no child nodes.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Data.IsEmpty;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Stanza class.
        /// </summary>
        /// <param name="namespace">The xml namespace of the stanza, if any.</param>
        /// <param name="to">The JID of the intended recipient for the stanza.</param>
        /// <param name="from">The JID of the sender.</param>
        /// <param name="id">The ID of the stanza.</param>
        /// <param name="language">The language of the XML character data of
        /// the stanza.</param>
        /// <param name="data">The content of the stanza.</param>
        public Stanza(string @namespace = null, Jid to = null,
            Jid from = null, string id = null, CultureInfo language = null,
            params XElement[] data)
        {
            var name = GetType().Name.ToLowerInvariant();
            element = Xml.Element(name, @namespace);
            To = to;
            From = from;
            Id = id;
            Language = language;
            foreach (XElement e in data)
            {
                if (e != null)
                    element.Child(e);
            }
        }

        /// <summary>
        /// Initializes a new instance of the Stanza class using the specified
        /// XElement.
        /// </summary>
        /// <param name="element">The XElement to create the stanza from.</param>
        /// <exception cref="ArgumentNullException">The element parameter is
        /// null.</exception>
        protected Stanza(XElement element)
        {
            element.ThrowIfNull("element");
            this.element = element;
        }

        /// <summary>
        /// Returns a textual representation of this instance of the Stanza class.
        /// </summary>
        /// <returns>A textual representation of this Stanza instance.</returns>
        public override string ToString()
        {
            return element.ToXmlString();
        }
    }
}