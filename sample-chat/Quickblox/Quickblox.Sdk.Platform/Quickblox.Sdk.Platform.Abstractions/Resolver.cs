using System;
using System.Collections.Generic;
using System.Globalization;

namespace Quickblox.Sdk.Platform
{
    public class Resolver
    {
        private static Dictionary<Type, Type> typeBindings = new Dictionary<Type, Type>();

        public static void Register<TBase, TImplementation>() where TImplementation : TBase, new()
        {
            typeBindings[typeof(TBase)] = typeof(TImplementation);
        }

        public static TImplementation Resolve<TImplementation>()
        {
            var sourceType = typeof(TImplementation);
            Type result = default(Type);
            bool boundExist = typeBindings.TryGetValue(sourceType, out result);
            if (!boundExist)
            {
                var errorMessage = String.Format(CultureInfo.InvariantCulture, "Sdk was not initialized. Call QuickbloxPlatform.Init() method before creating instance of Quickblox client", sourceType.FullName);
                throw new InvalidOperationException(errorMessage);
            }

            return (TImplementation)Activator.CreateInstance(result);
        }
    }
}