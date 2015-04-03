using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ChatModule.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
