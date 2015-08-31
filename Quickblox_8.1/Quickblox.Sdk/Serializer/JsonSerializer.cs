using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Quickblox.Sdk.Serializer
{
    /// <summary>
    /// Serializer class.
    /// </summary>
    internal class JsonSerializer : ISerializer
    {
        private List<Type> knownTypes = new List<Type>();

        public JsonSerializer()
        {
            
        }

        public JsonSerializer(IList<Type> knownTypes)
        {
            foreach (var knownType in knownTypes)
            {
                this.KnownTypes.Add(knownType);
            }
        }

        public List<Type> KnownTypes
        {
            get
            {
                return this.knownTypes;
            }
        }

        public T Deserialize<T>(String content)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), this.knownTypes);
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                return (T)serializer.ReadObject(memoryStream);
            }
        }

        public string ContentType
        {
            get { return "application/json"; }
        }

        public String Serialize<T>(T storedObj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), this.knownTypes);
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, storedObj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new StreamReader(memoryStream).ReadToEnd();
            }
        }
    }
}
