using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class NotificationChannel
    {
        [JsonProperty("name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationChannelType Name { get; set; }
    }
}
