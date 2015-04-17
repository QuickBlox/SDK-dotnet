using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class CreateEvent
    {
        public CreateEvent()
        {
            
        }

        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsActive { get; set; }

        [JsonProperty("notification_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType NotificationType { get; set; }

        [JsonProperty("push_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public PushType? PushType { get; set; }

        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserWithTags User { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? Date { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? EndDate { get; set; }

        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan? Period { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public String EventName { get; set; }

        [JsonProperty("external_user", NullValueHandling = NullValueHandling.Ignore)]
        public User ExternalUserId { get; set; }

        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public EventType? EventType { get; set; }
    }

}
