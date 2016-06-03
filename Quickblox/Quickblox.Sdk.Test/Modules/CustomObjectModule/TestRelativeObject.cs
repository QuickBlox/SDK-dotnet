using System;
using Newtonsoft.Json;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Test.Modules.CustomObjectModule
{
    public class TestRelativeObject : BaseCustomObject
    {
        [JsonProperty(PropertyName = "Location")]
        public Object Location { get; set; }

        [JsonProperty(PropertyName = "RelativeString")]
        public String RelativeString { get; set; }

        [JsonProperty(PropertyName = "FileField")]
        public String FileField { get; set; }
    }
}
