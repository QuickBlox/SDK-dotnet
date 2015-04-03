using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class Subscription
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("notification_channel")]
        public NotificationChannel NotificationChannel { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }
    }
}
