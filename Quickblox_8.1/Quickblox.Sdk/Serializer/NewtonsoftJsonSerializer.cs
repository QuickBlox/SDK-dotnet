using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Serializer
{
    internal class NewtonsoftJsonSerializer : ISerializer
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public T Deserialize<T>(String content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public String Serialize<T>(T storedObj)
        {
            return JsonConvert.SerializeObject(storedObj);
        }
    }
}
