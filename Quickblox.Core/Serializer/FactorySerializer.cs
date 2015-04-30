using System;

namespace Quickblox.Sdk.Serializer
{
    public class FactorySerializer : IFactorySerializer
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
            else
            {
                throw new NotImplementedException(String.Concat("Not found serializer for this content type:", contentType));
            }
        }

        public ISerializer CreateSerializer()
        {
            return new NewtonsoftJsonSerializer();
        }
    }
}
