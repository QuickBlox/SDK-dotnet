using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{

    public class Event
    {

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("event_type")]
        public EventType EventType { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }

        [JsonProperty("occured_count")]
        public int OccuredCount { get; set; }

        [JsonProperty("period")]
        public object Period { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("notification_channel")]
        public NotificationChannel NotificationChannel { get; set; }

        [JsonProperty("subscribers_selector")]
        public SubscribersSelector SubscribersSelector { get; set; }
    }

}
