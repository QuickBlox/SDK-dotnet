using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class CreateSubscriptionsRequest : BaseRequestSettings
    {
        [JsonProperty("push_token", NullValueHandling = NullValueHandling.Ignore)]
        public PushToken PushToken { get; set; }

        [JsonProperty("device", NullValueHandling = NullValueHandling.Ignore)]
        public DeviceRequest DeviceRequest { get; set; }

        [JsonProperty("notification_channels")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationChannelType Name { get; set; }
    }
}
