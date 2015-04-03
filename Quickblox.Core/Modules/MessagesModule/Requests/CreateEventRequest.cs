using Newtonsoft.Json;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Requests
{
    public class CreateEventRequest : BaseRequestSettings
    {
        [JsonProperty("event")]
        public CreateEvent Event { get; set; }
    }

}
