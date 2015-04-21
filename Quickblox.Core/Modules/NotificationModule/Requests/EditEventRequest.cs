using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class EditEventRequest
    {
        [JsonProperty(PropertyName = "event")]
        public EditEvent EditEvent { get; set; }
    }
}
