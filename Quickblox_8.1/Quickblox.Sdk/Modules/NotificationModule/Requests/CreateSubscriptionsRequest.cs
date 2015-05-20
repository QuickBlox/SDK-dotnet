using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Requests
{
    public class CreateSubscriptionsRequest : BaseRequestSettings
    {
        [JsonProperty("notification_channels")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationChannelType Name { get; set; }
    }
}
