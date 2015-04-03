using Newtonsoft.Json;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Requests
{
    public class CreatePushTokenRequest : BaseRequestSettings
    {
        [JsonProperty("push_token")]
        public PushToken PushToken { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }
    }
}
