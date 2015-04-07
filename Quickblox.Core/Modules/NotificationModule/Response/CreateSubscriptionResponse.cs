using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Response
{
    public class CreateSubscriptionResponse
    {
        [JsonProperty("subscription")]
        public Subscription Subscription { get; set; }
    }
}
