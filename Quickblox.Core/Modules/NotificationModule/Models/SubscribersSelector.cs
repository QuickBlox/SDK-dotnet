using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{

    public class SubscribersSelector
    {

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("user_ids")]
        public int[] UserIds { get; set; }
    }

}
