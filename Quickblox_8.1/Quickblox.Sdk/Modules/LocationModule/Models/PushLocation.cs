using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.LocationModule.Models
{
    /// <summary>
    /// Create geodata with the location-push
    /// </summary>
    public class PushLocation : Location
    {
        /// <summary>
        /// The distance in meters around your coordinates
        /// </summary>
        [JsonProperty(PropertyName = "radius")]
        public UInt32 Radius { get; set; }

        /// <summary>
        /// Should be Base64 encoded text.
        /// </summary>
        [JsonProperty(PropertyName = "push_message")]
        public String PushMessage { get; set; }

        /// <summary>
        /// Environment of the notification
        /// </summary>
        [JsonProperty(PropertyName = "push_environment")]
        public PushEnvironment PushEnvironment { get; set; }
    }

    /// <summary>
    /// Environment of the notification
    /// </summary>
    public enum PushEnvironment
    {
        development,
        production
    }
}
