using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    public class DeviceRequest
    {
        [JsonProperty("platform")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Platform { get; set; }

        [JsonProperty("udid")]
        public string Udid { get; set; }
    }

    public class DeviceResponse
    {
        [JsonProperty("platform")]
        public PlatformName Platform { get; set; }

        [JsonProperty("udid")]
        public string Udid { get; set; }
    }

    public class PlatformName
    {
        [JsonProperty("name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Platform { get; set; }
    }

    public enum Platform
    {
        ios,
        android,
        windows_phone,
        blackberry
    }
}
