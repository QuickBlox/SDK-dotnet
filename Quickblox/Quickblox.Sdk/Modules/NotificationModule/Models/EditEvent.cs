using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class EditEvent
    {
        /// <summary>
        /// Marks event as active/inactive
        /// </summary>
        [JsonProperty(PropertyName = "active", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsActive { get; set; }

        /// <summary>
        /// Buid XmppMessage in base64 data with reqqired parameters
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        [JsonConverter(typeof(MessageConverter))]
        public IMessage Message { get; set; }

        /// <summary>
        /// The date of the event.
        /// If the 'event type'=='fixed_date', the date can not be in the past.
        /// </summary>
        [JsonProperty(PropertyName = "date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Date { get; set; }

        /// <summary>
        /// The period of the event in seconds.
        /// Possible values:
        /// 86400 (1 day)
        /// 604800 (1 week)
        /// 2592000 (1 month)
        /// 31557600 (1 year)
        /// </summary>
        [JsonProperty(PropertyName = "period", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Period { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public String EventName { get; set; }
    }
}
