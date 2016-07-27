using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.AuthModule.Models
{
    public class Session
    {
        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("device_id")]
        public object DeviceId { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("nonce")]
        public int Nonce { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("ts")]
        public int Ts { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
