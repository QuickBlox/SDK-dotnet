using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class PushToken
    {
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        [JsonProperty("client_identification_sequence", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIdentificationSequence { get; set; }
        
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string PushTokenId { get; set; }
    }

    public enum Environment
    {
        development,
        production
    }
}
