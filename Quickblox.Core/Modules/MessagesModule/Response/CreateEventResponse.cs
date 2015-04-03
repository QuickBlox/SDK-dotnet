using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{

    public class CreateEventResponse
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }

}
