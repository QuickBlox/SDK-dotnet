// TagRegistry.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XMPP.tags;
using XMPP.tags.bosh;

namespace XMPP.registries
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class XMPPTagAttribute : Attribute
    {
        public readonly Type Type;
        public readonly XName Name;
        public string LocalName { get { return this.Name.LocalName; } }
        public string Namespace { get { return this.Name.NamespaceName; } }

        public XMPPTagAttribute(Type namespaceType, Type elementType)
        {
            if (namespaceType != null && namespaceType.Name == "Namespace")
            {
                foreach (var field in namespaceType.GetTypeInfo().DeclaredFields)
                {
                    if (field.IsStatic && field.Name == elementType.Name)
                        this.Name = (XName)field.GetValue(null);
                }

                this.Type = elementType;
            }
        }
    }

	public class TagRegistry
	{
		public TagRegistry()
		{
            AddAssembly(typeof(Client).GetTypeInfo().Assembly);
		}

        private Dictionary<XName, Type> RegisteredItems = new Dictionary<XName, Type>();

		public void AddAssembly(Assembly ass)
		{
			var tags = GetAttributes<XMPPTagAttribute>(ass);
			foreach (var tag in tags)
			{
				RegisteredItems.Add(tag.Name, tag.Type);
			}
		}

        public XName GetName(Type type)
        {
            if( RegisteredItems.ContainsValue(type) )
            {
                foreach( KeyValuePair<XName, Type> element in RegisteredItems )
                {
                    if( element.Value == type)
                        return element.Key;
                }
            }
            return null;
        }

		public Tag GetTag(string name, string ns)
		{
			return GetTag(XName.Get(name, ns));
		}

		public Tag GetTag(XName name)
		{
			try
			{
				Type type;
                if (RegisteredItems.TryGetValue(name, out type))
				{
                    // Try doc and qname constructor
                    ConstructorInfo ctorName = GetConstructor(type, new Type[] { name.GetType() });
                    if (ctorName != null)
                        return ctorName.Invoke(new object[] { name }) as Tag;

                    // Try empty constructor
                    ConstructorInfo ctorDefault = GetConstructor(type, new Type[] { });
                    if (ctorDefault != null)
                        return ctorDefault.Invoke(new object[] { }) as Tag;
				}

				return null;
			}
			catch
			{
                return null;
			}
		}

        public Tag GetTag(XElement element)
        {
            try
            {
                Type type;

                bool gotType = RegisteredItems.TryGetValue(element.Name, out type);

                if (gotType)
                {
                    ConstructorInfo ctorName = GetConstructor(type, new Type[] { element.GetType() });
                    if (ctorName != null)
                        return ctorName.Invoke(new object[] { element }) as Tag;

                    ConstructorInfo ctorDefault = GetConstructor(element.GetType(), new Type[] { typeof(Tag) });
                    if (ctorDefault != null)
                        return ctorDefault.Invoke(new object[] { element }) as Tag;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        protected TE[] GetAttributes<TE>(Assembly ass)
        {
            var returns = new List<TE>();
            foreach (var type in ass.DefinedTypes)
            {
                IEnumerable<TE> attributes = (IEnumerable<TE>)type.GetCustomAttributes(typeof(TE), false);

                foreach (var attribute in attributes)
                {
                    returns.Add((TE)attribute);
                }
            }

            return returns.ToArray();
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

	}
}