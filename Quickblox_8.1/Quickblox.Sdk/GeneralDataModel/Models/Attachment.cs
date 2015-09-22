using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
