using System;

namespace Quickblox.Sdk.Serializer
{
    internal class FactorySerializer : IFactorySerializer
    {
        private const string JsonContentType = "application/json";
        private const string XmlContentType = "application/xml";

        public ISerializer CreateSerializer(string contentType)
        {
            if (contentType.Contains(JsonContentType))
            {
                return new NewtonsoftJsonSerializer();
            }
            if (contentType.Contains(XmlContentType))
            {
                return new XmlSerializer();
            }

            return null;
        }

        public ISerializer CreateSerializer()
        {
            return new NewtonsoftJsonSerializer();
        }
    }
}
