using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class CreateEventRequest : BaseRequestSettings
    {
        [JsonProperty("event")]
        public CreateEvent Event { get; set; }
    }

}
