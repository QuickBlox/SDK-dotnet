﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Xmpp.Im
{
    /// <summary>
    /// Represents a XmppMessage stanza as defined in XMPP:IM.
    /// </summary>
    public class XmppMessage : Core.Message
    {
        /// <summary>
        /// The type of the XmppMessage stanza.
        /// </summary>
        private MessageType type;

        /// <summary>
        /// The time at which the XmppMessage was originally sent.
        /// </summary>
        private DateTime timestamp = DateTime.Now;

        /// <summary>
        /// The type of the XmppMessage stanza.
        /// </summary>
        public MessageType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                var v = value.ToString().ToLowerInvariant();
                element.SetAttribute("type", v);
            }
        }

        /// <summary>
        /// The time at which the XmppMessage was originally sent.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                // Refer to XEP-0203.
                var delay = element.Element("delay");
                if (delay != null && delay.GetDefaultNamespace().NamespaceName == "urn:xmpp:delay")
                {
                    DateTime result;
                    if (DateTime.TryParse(delay.GetAttribute("stamp"), out result))
                        return result;
                }
                return timestamp;
            }
        }

        /// <summary>
        /// The conversation thread this XmppMessage belongs to.
        /// </summary>
        public string Thread
        {
            get
            {
                if (element.Element("thread") != null)
                    return element.Element("thread").Value;
                return null;
            }

            set
            {
                var e = element.Element("thread");
                if (e != null)
                {
                    if (value == null)
                        element.RemoveChild(e);
                    else
                        e.Value = value;
                }
                else
                {
                    if (value != null)
                        element.Child(Xml.Element("thread").Text(value));
                }
            }
        }

        /// <summary>
        /// The subject of the XmppMessage.
        /// </summary>
        public string Subject
        {
            get
            {
                XElement bare = GetBare("subject");
                if (bare != null)
                    return bare.Value;
                string k = AlternateSubjects.Keys.FirstOrDefault();
                return k != null ? AlternateSubjects[k] : null;
            }

            set
            {
                XElement bare = GetBare("subject");
                if (bare != null)
                {
                    if (value == null)
                        element.RemoveChild(bare);
                    else
                        bare.Value = value;
                }
                else
                {
                    if (value != null)
                        element.Child(Xml.Element("subject").Text(value));
                }
            }
        }

        /// <summary>
        /// The body of the XmppMessage.
        /// </summary>
        public string Body
        {
            get
            {
                XElement bare = GetBare("body");
                if (bare != null)
                    return bare.Value;
                string k = AlternateBodies.Keys.FirstOrDefault();
                return k != null ? AlternateBodies[k] : null;
            }

            set
            {
                XElement bare = GetBare("body");
                if (bare != null)
                {
                    if (value == null)
                        element.RemoveChild(bare);
                    else
                        bare.Value = value;
                }
                else
                {
                    if (value != null)
                        element.Child(Xml.Element("body").Text(value));
                }
            }
        }

        public XElement ExtraParameters
        {
            get
            {
                return GetBare("extraParams");
            }
            set
            {
                XElement bare = GetBare("extraParams");
                if (bare != null)
                {
                    if (value == null)
                        element.RemoveChild(bare);
                    else
                    {
                        element.Child(value);
                    }
                }
                else
                {
                    if (value != null)
                    {
                        element.Child(value);
                    }
                }
            }
        }

        /// <summary>
        /// A dictionary of alternate forms of the XmppMessage subjects. The keys of the
        /// dictionary denote ISO 2 language codes.
        /// </summary>
        public IDictionary<string, string> AlternateSubjects
        {
            get;
            private set;
        }

        /// <summary>
        /// A dictionary of alternate forms of the XmppMessage bodies. The keys of the
        /// dictionary denote ISO 2 language codes.
        /// </summary>
        public IDictionary<string, string> AlternateBodies
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the XmppMessage class.
        /// </summary>
        /// <param name="to">The JID of the intended recipient.</param>
        /// <param name="body">The content of the XmppMessage.</param>
        /// <param name="subject">The subject of the XmppMessage.</param>
        /// <param name="thread">The conversation thread this XmppMessage belongs to.</param>
        /// <param name="type">The type of the XmppMessage. Can be one of the values from
        /// the MessagType enumeration.</param>
        /// <param name="language">The language of the XML character data of
        /// the stanza.</param>
        /// <exception cref="ArgumentNullException">The to parameter is null.</exception>
        /// <exception cref="ArgumentException">The body parameter is the empty string.</exception>
        public XmppMessage(Jid to, string id = null,string body = null, string subject = null, string thread = null, XElement extraParams = null,
            MessageType type = MessageType.Normal, CultureInfo language = null)
            : base(to, null, null, null, language)
        {
            to.ThrowIfNull("to");
            AlternateSubjects = new XmlDictionary(element, "subject", "xml:lang");
            AlternateBodies = new XmlDictionary(element, "body", "xml:lang");
            this.ExtraParameters = extraParams;
            Id = id;
            Type = type;
            Body = body;
            Subject = subject;
            Thread = thread;
        }

        ///// <summary>
        ///// Initializes a new instance of the XmppMessage class.
        ///// </summary>
        ///// <param name="to">The JID of the intended recipient.</param>
        ///// <param name="bodies">A dictionary of XmppMessage bodies. The dictionary
        ///// keys denote the languages of the XmppMessage bodies and must be valid
        ///// ISO 2 letter language codes.</param>
        ///// <param name="subjects">A dictionary of XmppMessage subjects. The dictionary
        ///// keys denote the languages of the XmppMessage subjects and must be valid
        ///// ISO 2 letter language codes.</param>
        ///// <param name="thread">The conversation thread this XmppMessage belongs to.</param>
        ///// <param name="type">The type of the XmppMessage. Can be one of the values from
        ///// the MessagType enumeration.</param>
        ///// <param name="language">The language of the XML character data of
        ///// the stanza.</param>
        ///// <exception cref="ArgumentNullException">The to parametr or the bodies
        ///// parameter is null.</exception>
        //public XmppMessage(Jid to, IDictionary<string, string> bodies,
        //    IDictionary<string, string> subjects = null, string thread = null, string extraParams = null,
        //    MessageType type = MessageType.Normal, CultureInfo language = null)
        //    : base(to, null, null, null, language)
        //{
        //    to.ThrowIfNull("to");
        //    bodies.ThrowIfNull("bodies");
        //    AlternateSubjects = new XmlDictionary(element, "subject", "xml:lang");
        //    AlternateBodies = new XmlDictionary(element, "body", "xml:lang");
        //    this.ExtraParameters = extraParams;
        //    Type = type;
        //    foreach (var pair in bodies)
        //        AlternateBodies.Add(pair.Key, pair.Value);
        //    if (subjects != null)
        //    {
        //        foreach (var pair in subjects)
        //            AlternateSubjects.Add(pair.Key, pair.Value);
        //    }
        //    Thread = thread;
        //}

        /// <summary>
        /// Initializes a new instance of the XmppMessage class from the specified
        /// instance.
        /// </summary>
        /// <param name="message">An instance of the Core.XmppMessage class to
        /// initialize this instance with.</param>
        /// <exception cref="ArgumentNullException">The XmppMessage parameter is null.</exception>
        /// <exception cref="ArgumentException">The 'type' attribute of
        /// the specified XmppMessage stanza is invalid.</exception>
        internal XmppMessage(Core.Message message)
        {
            message.ThrowIfNull("message");
            type = ParseType(message.Data.GetAttribute("type"));
            element = message.Data;
            AlternateSubjects = new XmlDictionary(element, "subject", "xml:lang");
            AlternateBodies = new XmlDictionary(element, "body", "xml:lang");
        }

        /// <summary>
        /// Parses the XmppMessage type from the specified string.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <returns>The MessageType value parsed from the string.</returns>
        /// <exception cref="ArgumentException">The specified value for the stanza
        /// type is invalid.</exception>
        private MessageType ParseType(string value)
        {
            // The 'type' attribute of XmppMessage-stanzas is optional and if absent
            // a type of 'normal' is assumed.
            if (String.IsNullOrEmpty(value))
                return MessageType.Normal;
            return (MessageType)Enum.Parse(typeof(MessageType),
                value.Capitalize());
        }

        /// <summary>
        /// Attempts to retrieve the bare element (i.e. without an xml:lang
        /// attribute) with the specified tag name.
        /// </summary>
        /// <param name="tag">The tag name of the element to retrieve.</param>
        /// <returns>The located element or null if no such element exists.</returns>
        private XElement GetBare(string tag)
        {
            foreach (XElement e in element.Descendants(tag))
            {
                string k = e.GetAttribute(XName.Get("lang", XNamespace.Xml.NamespaceName));
                if (String.IsNullOrEmpty(k))
                    return e;
            }
            return null;
        }
    }
}