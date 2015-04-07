using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Response
{
    public class CreatePushTokenResponse
    {
        [JsonProperty("push_token")]
        public PushToken PushToken { get; set; }
    }
}
