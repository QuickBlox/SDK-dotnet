using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel
{
    public class ErrorResponse
    {
        [JsonProperty("errors")]
        public Error Error { get; set; }
    }
}
