using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class CreateEvent
    {
        public CreateEvent()
        {
            this.PushType = PushType.mpns;
        }

        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsActive { get; set; }

        [JsonProperty("notification_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType NotificationType { get; set; }

        [JsonProperty("push_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PushType PushType { get; set; }

        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserWithTags User { get; set; }

        [JsonProperty("message")]
        [JsonConverter(typeof(MessageConverter))]
        public IMessage Message { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Date { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? EndDate { get; set; }

        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Period { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public String EventName { get; set; }

        [JsonProperty("external_user", NullValueHandling = NullValueHandling.Ignore)]
        public User ExternalUserId { get; set; }

        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public EventType? EventType { get; set; }
    }

}
