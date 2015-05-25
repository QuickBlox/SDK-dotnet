using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Models
{
    public class CustomObjectItems<T> where T : BaseCustomObject
    {
        [JsonProperty(PropertyName = "class_name")]
        public String ClassName { get; set; }

        [JsonProperty(PropertyName = "skip")]
        public Int64 Skip { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public Int64 Limit { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<T> Items { get; set; }

        [JsonProperty(PropertyName = "not_found")]
        public NotFoundsItems NotFoundsItems { get; set; }
    }
}
