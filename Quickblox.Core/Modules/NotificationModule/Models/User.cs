using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{

    public class User
    {
        [JsonProperty("ids")]
        public string Ids { get; set; }
    }
}
