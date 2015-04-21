using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    public class DeviceRequest
    {
        /// <summary>
        /// Platform of device, which is the source of API requests to Quickblox
        /// </summary>
        [JsonProperty("platform")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Platform { get; set; }

        /// <summary>
        /// UDID (Unique Device identifier) of device, which is the source of API requests to Quickblox. This must be anything sequence which uniquely identify particular device. This is needed to support schema: 1 User - Multiple devices.
        /// </summary>
        [JsonProperty("udid")]
        public string Udid { get; set; }
    }

    public class DeviceResponse
    {
        /// <summary>
        /// Platform of device, which is the source of API requests to Quickblox.
        /// </summary>
        [JsonProperty("platform")]
        public PlatformName Platform { get; set; }

        /// <summary>
        /// UDID (Unique Device identifier) of device, which is the source of API requests to Quickblox. This must be anything sequence which uniquely identify particular device. This is needed to support schema: 1 User - Multiple devices.
        /// </summary>
        [JsonProperty("udid")]
        public string Udid { get; set; }
    }

    public class PlatformName
    {
        /// <summary>
        /// Platform of device, which is the source of API requests to Quickblox.
        /// </summary>
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
