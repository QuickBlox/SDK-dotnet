using System.Collections.Generic;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class ErrorResponse
    {
        [JsonProperty("errors")]
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
