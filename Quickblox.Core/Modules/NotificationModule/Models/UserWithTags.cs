using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class UserWithTags : User
    {
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public Tags Tags { get; set; }
    }
}
