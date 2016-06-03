using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Responses
{
    public class GetSubscriptionResponse
    {
        [JsonProperty("subscription")]
        public Subscription Subscription { get; set; }
    }
}
