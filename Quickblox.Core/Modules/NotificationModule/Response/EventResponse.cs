using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{

    public class EventResponse
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }

}
