using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class ErrorResponse
    {
        [JsonProperty("errors")]
        public Error Error { get; set; }
    }
}
