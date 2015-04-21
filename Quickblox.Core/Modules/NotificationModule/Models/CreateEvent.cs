using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class CreateEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEvent"/> class.
        /// </summary>
        public CreateEvent()
        {
            this.PushType = PushType.mpns;
        }

        /// <summary>
        /// If you want to send specific notification more than once - just edit event & set this field to 'true', Then push will be send immediately, without creating a new one event.
        /// By default: true
        /// </summary>
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsActive { get; set; }

        /// <summary>
        /// push: Push notification
        /// email
        /// </summary>
        [JsonProperty("notification_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Used only if notification_type == push, ignored in other cases
        /// If not present - Notification will be delivered to all possible devices for specified users.Each platform will have their own standard format.See Quickblox Standard Push Notifications Formats for more information
        /// If specified - Notification will be delivered to specified platform only
        /// </summary>
        [JsonProperty("push_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PushType PushType { get; set; }

        /// <summary>
        /// Environment of the notification..
        /// </summary>
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment Environment { get; set; }

        /// <summary>
        /// Filter by user parameters.
        /// </summary>
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserWithTags User { get; set; }

        /// <summary>
        /// Pushes: event[push_type] not present - should be Base64 encoded text.
        ///         event[push_type] specified - should be formatted as described in QuickBlox Push Notifications Formats
        /// Email:  Base64 encoded tex
        /// </summary>
        [JsonProperty("message")]
        [JsonConverter(typeof(MessageConverter))]
        public IMessage Message { get; set; }

        /// <summary>
        /// The date of the event. If the 'event type'=='fixed_date', the date can not be in the past.
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Date { get; set; }

        /// <summary>
        /// Date of completion of the event. Can't be less than the 'date'.
        /// </summary>
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? EndDate { get; set; }

        /// <summary>
        /// The period of the event in seconds.
        /// Possible values:
        /// 86400 (1 day)
        /// 604800 (1 week)
        /// 2592000 (1 month)
        /// 31557600 (1 year)
        /// </summary>
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Period { get; set; }

        /// <summary>
        /// The name of the event. Service information. Only for the user.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public String EventName { get; set; }

        /// <summary>
        /// Should contain a string of external users' ids divided by commas.
        /// </summary>
        /// <value>
        /// The external user identifier.
        /// </value>
        [JsonProperty("external_user", NullValueHandling = NullValueHandling.Ignore)]
        public User ExternalUserId { get; set; }

        /// <summary>
        /// one_shot - a one-time event, which causes by an external object (the value is only valid if the 'date' is not specified)
        /// fixed_date - a one-time event, which occurs at a specified 'date' (the value is valid only if the 'date' is given)
        /// period_date - reusable event that occurs within a given 'period' from the initial 'date' (the value is only valid if the 'period' specified)
        /// By default:
        /// fixed_date, if 'date' is specified
        /// period_date, if 'period' is specified
        /// one_shot, if 'date' is not specified
        /// </summary>
        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public EventType? EventType { get; set; }
    }

}
