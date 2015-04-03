using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.CoreModule.Response
{
    public class LoginResponse
    {
        [JsonProperty("blob_id")]
        public object BlobId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("custom_parameters")]
        public object CustomParameters { get; set; }

        [JsonProperty("email")]
        public object Email { get; set; }

        [JsonProperty("external_user_id")]
        public int ExternalUserId { get; set; }

        [JsonProperty("facebook_id")]
        public object FacebookId { get; set; }

        [JsonProperty("full_name")]
        public object FullName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("last_request_at")]
        public string LastRequestAt { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("phone")]
        public object Phone { get; set; }

        [JsonProperty("twitter_id")]
        public object TwitterId { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("website")]
        public object Website { get; set; }

        [JsonProperty("user_tags")]
        public string UserTags { get; set; }
    }
}
