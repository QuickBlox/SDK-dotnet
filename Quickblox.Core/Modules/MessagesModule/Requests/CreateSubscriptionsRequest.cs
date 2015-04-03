using Newtonsoft.Json;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Requests
{
    public class CreateSubscriptionsRequest : BaseRequestSettings
    {
        [JsonProperty("notification_channels")]
        public NotificationChannel Channel { get; set; }
    }
}
