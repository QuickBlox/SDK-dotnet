using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class PushToken
    {
        /// <summary>
        /// Determine application mode. It allows conveniently separate development and production modes. Allowed values: development, production
        /// </summary>
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        /// <summary>
        /// Identifies client device in 3-rd party service like APNS, GCM, BBPS or MPNS. Initially retrieved from 3-rd service and should be send to QuickBlox to let it send push notifications to the client.
        /// </summary>
        [JsonProperty("client_identification_sequence", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIdentificationSequence { get; set; }

        /// <summary>
        /// Generated push token identifier by quickblox server.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string PushTokenId { get; set; }
    }

    public enum Environment
    {
        development,
        production
    }
}
