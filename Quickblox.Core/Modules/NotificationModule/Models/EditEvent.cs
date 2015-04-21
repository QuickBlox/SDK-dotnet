using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class EditEvent
    {
        [JsonProperty(PropertyName = "active", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsActive { get; set; }

        [JsonProperty(PropertyName = "message")]
        [JsonConverter(typeof(MessageConverter))]
        public IMessage Message { get; set; }

        [JsonProperty(PropertyName = "date", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Date { get; set; }

        [JsonProperty(PropertyName = "period", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Period { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public String EventName { get; set; }
    }
}
