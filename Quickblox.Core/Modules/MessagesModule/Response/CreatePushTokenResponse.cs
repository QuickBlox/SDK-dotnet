using Newtonsoft.Json;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Response
{
    public class CreatePushTokenResponse
    {
        [JsonProperty("push_token")]
        public PushToken PushToken { get; set; }
    }
}
