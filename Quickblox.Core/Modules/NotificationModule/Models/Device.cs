using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class Device
    {
        [JsonProperty("platform")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Platform { get; set; }

        [JsonProperty("udid")]
        public string Udid { get; set; }
    }

    public enum Platform
    {
        ios,
        android,
        windows_phone,
        blackberry
    }
}
