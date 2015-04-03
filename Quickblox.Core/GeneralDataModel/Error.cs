using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel
{
    public class Error
    {
        [JsonProperty("base")]
        public String[] Text { get; set; }
    }
}
