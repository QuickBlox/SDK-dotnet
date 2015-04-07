using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class CreateSubscriptionsRequest : BaseRequestSettings
    {
        [JsonProperty("notification_channels")]
        public NotificationChannel Channel { get; set; }
    }
}
