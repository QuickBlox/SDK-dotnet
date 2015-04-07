using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{

    public class CreateEventResponse
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }

}
