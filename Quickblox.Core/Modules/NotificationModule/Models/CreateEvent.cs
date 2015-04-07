using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class CreateEvent
    {
        [JsonProperty("notification_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType NotificationType { get; set; }

        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("push_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PushType PushType { get; set; }

        [JsonProperty("name")]
        public String EventName { get; set; }
    }

}
