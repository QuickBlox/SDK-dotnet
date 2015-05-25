// Tag.cs
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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using XMPP.registries;
using System.Runtime.Serialization;

namespace XMPP.tags
{
    public class Tag : XElement
    {
        protected Tag(XName identity) : base(identity) { }
        protected Tag(XElement other) : base(other) { }

        private static int _packetCounter;

        public DateTime Timestamp
        {
            get
            {
                foreach (var node in Nodes())
                {
                    if (node.NodeType == XmlNodeType.Comment)
                    {
                        var comment = node as XComment;
                        var pos = comment.Value.IndexOf("[TS]");
                        if (pos != -1)
                            return DateTime.Parse(comment.Value.Substring(pos + 4));
                    }
                }
                return default(DateTime);
            }

            set
            {
                if( Timestamp == default(DateTime) )
                    Add( new XComment( "[TS]" + DateTime.Now.ToString() ) );
            }
        }

        public string Account
        {
            get
            {
                foreach (var node in Nodes())
                {
                    if (node.NodeType == XmlNodeType.Comment)
                    {
                        var comment = node as XComment;
                        var pos = comment.Value.IndexOf("[AC]");
                        if (pos != -1)
                            return comment.Value.Substring(pos + 4);
                    }
                }
                return string.Empty;
            }

            set
            {
                if (string.IsNullOrEmpty(Account))
                    Add(new XComment("[AC]" + value.ToString()));
            }
        }


        public static string NextId()
        {
            return "U" + Interlocked.Increment(ref _packetCounter);
        }

        public byte[] Bytes
        {
            get { return System.Convert.FromBase64String(Value); }
            set { Value = System.Convert.ToBase64String(value); }
        }

        public static implicit operator string(Tag tag)
        {
            return tag.ToString();
        }

        public object GetAttributeValue(XName name)
        {
            XAttribute attr = Attribute(name);
            if (attr != null)
                return attr.Value;
            else
                return default(object);
        }

        public int? GetAttributeValueAsInt(XName name)
        {
            var value = GetAttributeValue(name);

            if (null != value)
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }

            return null;
        }

        public long? GetAttributeValueAsLong(XName name)
        {
            var value = GetAttributeValue(name);

            if (null != value)
            {
                long result;
                if (long.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }

            return null;
        }

        public bool? GetAttributeValueAsBool(XName name)
        {
            var value = GetAttributeValue(name);

            if (null != value)
            {
                bool result;
                if (bool.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }

            return null;
        }

        public T GetAttributeEnum<T>(XName name)
        {
            var attr = (string)GetAttributeValue(name);
            if (attr != null)
            {
                var enumType = typeof(T);
                foreach (var item in Enum.GetNames(enumType))
                {
                    var enumMember = ((EnumMemberAttribute[])enumType.GetRuntimeField(item).GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
                    if (enumMember != null && enumMember.Value == attr)
                    {
                        return (T)Enum.Parse(enumType, item);
                    }
                }

                return (T) Enum.Parse(typeof(T), attr, true);
            }

            return default(T);
        }

        public void SetAttributeEnum<T>(XName name, object value)
        {
            object obj = Enum.ToObject(typeof(T), value);

            IDictionary<string, MemberInfo> members = obj.GetType().GetTypeInfo().DeclaredMembers.ToDictionary(c => c.Name);

            MemberInfo member;
            if (!members.TryGetValue(obj.ToString(), out member))
            {
                throw new InvalidOperationException();
            }

            IDictionary<Type, CustomAttributeData> customAttributes = member.CustomAttributes.ToDictionary(c => c.AttributeType);

            CustomAttributeData enumMemberAttribute;
            if (customAttributes.TryGetValue(typeof(EnumMemberAttribute), out enumMemberAttribute))
            {
                IDictionary<string, CustomAttributeNamedArgument> args = enumMemberAttribute.NamedArguments.ToDictionary(c => c.MemberName);

                CustomAttributeNamedArgument arg;
                if (args.TryGetValue("Value", out arg))
                {
                    SetAttributeValue(name, arg.TypedValue.Value);
                    return;
                }
            }

            SetAttributeValue(name, ((T)value).ToString());
        }

        public new IEnumerable<Tag> Descendants() { return Descendants<Tag>(); }
        public IEnumerable<T> Descendants<T>() where T : XElement
        {
            return base.Descendants().Select(descendant => Convert<T>(descendant));
        }


        public new IEnumerable<Tag> Descendants(XName name) { return Descendants<Tag>(name); }
        public IEnumerable<T> Descendants<T>(XName name) where T : XElement
        {
            return base.Descendants(name).Select(descendant => Convert<T>(descendant));
        }


        public new Tag Element(XName name) { return Element<Tag>(name); }
        public T Element<T>(XName name) where T : XElement
        {
            return Convert<T>(base.Element(name));
        }

        public new IEnumerable<XElement> Elements() { return Elements<Tag>(); }
        public IEnumerable<T> Elements<T>() where T : XElement
        {
            return base.Elements().Select(element => Convert<T>(element));
        }

        public new IEnumerable<Tag> Elements(XName name) { return Elements<Tag>(name); }
        public IEnumerable<T> Elements<T>(XName name) where T : XElement
        {
            return base.Elements(name).Select(element => Convert<T>(element));
        }

        public new IEnumerable<Tag> AncestorsAndSelf() { return AncestorsAndSelf<Tag>(); }
        public IEnumerable<T> AncestorsAndSelf<T>() where T : XElement
        {
            return base.AncestorsAndSelf().Select(ancestor => Convert<T>(ancestor));
        }

        public new IEnumerable<Tag> AncestorsAndSelf(XName name) { return AncestorsAndSelf<Tag>(name); }
        public IEnumerable<T> AncestorsAndSelf<T>(XName name) where T : XElement
        {
            return base.AncestorsAndSelf(name).Select(ancestor => Convert<T>(ancestor));
        }

        public new IEnumerable<Tag> DescendantsAndSelf() { return DescendantsAndSelf<Tag>(); }
        public IEnumerable<T> DescendantsAndSelf<T>() where T : XElement
        {
            return base.DescendantsAndSelf().Select(descendant => Convert<T>(descendant));
        }

        public new IEnumerable<Tag> DescendantsAndSelf(XName name) { return DescendantsAndSelf<Tag>(name); }
        public IEnumerable<T> DescendantsAndSelf<T>(XName name) where T : XElement
        {
            return base.DescendantsAndSelf(name).Select(descendant => Convert<T>(descendant));
        }

        public ConstructorInfo GetConstructor(Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
        }

        public ConstructorInfo GetConstructor(Type type, bool isStatic, bool isPrivate)
        {
            var results = type.GetTypeInfo().DeclaredConstructors;

            foreach (var result in results)
            {
                if (result.IsStatic == isStatic && result.IsPrivate == isPrivate)
                    return result;
            }

            return null;
        }

        public ConstructorInfo GetConstructor(Type type, Type[] parameters)
        {
            var results = from constructor in type.GetTypeInfo().DeclaredConstructors
                          let constructorParameters = constructor.GetParameters().Select(_ => _.ParameterType).ToArray()
                          where constructorParameters.Length == parameters.Length &&
                                !constructorParameters.Except(parameters).Any() &&
                                !parameters.Except(constructorParameters).Any()
                          select constructor;

            return results.FirstOrDefault();
        }

        private TReturn Convert<TReturn>(XElement element) where TReturn : XElement
        {
            if (element == null)
                return default(TReturn);

            var ctor = GetConstructor(typeof(TReturn), new Type[] { typeof(XElement) });
            if (ctor != null)
                return (TReturn)ctor.Invoke(new object[] { element });
            else
                return default(TReturn);

        }

        public static Tag Get(XElement element)
        {
            var tag = Static.TagRegistry.GetTag(element);

            if (tag != null)
            {
                if (tag.Timestamp == default(DateTime))
                    tag.Timestamp = DateTime.Now;
            }

            return tag;
        }

        public static Tag Get(XName name)
        {
            var tag = Static.TagRegistry.GetTag(name);

            if (tag.Timestamp == default(DateTime))
                tag.Timestamp = DateTime.Now;

            return tag;
        }

        public static Tag Get(string xml)
        {
            try
            {
                var document = new XDocument();
                var stringReader = new StringReader(xml);
                var xmlReader = XmlReader.Create(stringReader, Static.Settings, Static.Context);

                xmlReader.MoveToContent();
                while (xmlReader.ReadState != ReadState.EndOfFile)
                {
                    document.Add(XNode.ReadFrom(xmlReader));
                }

                FixNS(document.Root);

                return Tag.Get(document.Root);
            }
            catch
            {
                return null;
            }
        }

        private static void FixNS(XElement e)
        {
            if (e.Name.LocalName == "body" && e.Name.Namespace == "http://jabber.org/protocol/httpbind" && !e.Ancestors().Any())
            {
                if (e.HasElements)
                {
                    foreach (var chield in e.Descendants())
                    {
                        FixNS(chield);
                    }
                }
            }
            else if (e.Name.LocalName == "iq" ||
                e.Name.LocalName == "presence" ||
                e.Name.LocalName == "message" ||
                (e.Name.LocalName == "error" && e.Name.NamespaceName != streams.Namespace.Name) ||
                e.Name.LocalName == "body" ||
                e.Name.LocalName == "show")
            {
                e.Name = XName.Get(e.Name.LocalName, "jabber:client");

                if (e.HasElements)
                {
                    foreach (var chield in e.Descendants())
                    {
                        FixNS(chield);
                    }
                }
            }
        }
    }
}
