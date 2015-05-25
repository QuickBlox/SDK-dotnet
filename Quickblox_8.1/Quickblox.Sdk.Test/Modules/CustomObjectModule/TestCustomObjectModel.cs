using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Test.Modules.CustomObjectModule
{
    public class TestCustomObjectModel : BaseCustomObject
    {
        [JsonProperty(PropertyName = "IntegerField")]
        public Int32 IntegerField { get; set; }

        [JsonProperty(PropertyName = "FloatField")]
        public Double FloatField { get; set; }

        [JsonProperty(PropertyName = "BooleanField")]
        public Boolean BooleanField { get; set; }

        [JsonProperty(PropertyName = "StringField")]
        public String StringField { get; set; }

        [JsonProperty(PropertyName = "ArrayField")]
        public List<Int32> ArrayField { get; set; }
    }
}
