using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class CreatePushTokenRequest : BaseRequestSettings
    {
        [JsonProperty("push_token")]
        public PushToken PushToken { get; set; }

        [JsonProperty("device")]
        public DeviceRequest DeviceRequest { get; set; }
    }
}
