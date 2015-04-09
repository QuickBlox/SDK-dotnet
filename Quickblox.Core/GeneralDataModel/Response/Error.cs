using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class Error
    {
        [JsonProperty("base")]
        public String[] Text { get; set; }
    }
}
