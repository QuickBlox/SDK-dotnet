using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class PushToken
    {
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        [JsonProperty("client_identification_sequence")]
        public string ClientIdentificationSequence { get; set; }
        
        [JsonProperty("id")]
        public string PushTokenId { get; set; }
    }

    public enum Environment
    {
        Development,
        Production
    }
}
