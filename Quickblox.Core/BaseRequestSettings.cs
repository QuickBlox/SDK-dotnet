using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Quickblox.Sdk
{
    public abstract class BaseRequestSettings
    {
        /// <summary>
        /// Возвращает коллекцию хедеров и их значений.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public IDictionary<String, String> Headers { get; set; }
    }

   
}

