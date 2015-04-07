using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.UsersModule.Models
{
    /// <summary>
    /// User that is returned in response for user sign up.
    /// </summary>
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("last_request_at")]
        public string LastRequestAt { get; set; }

        [JsonProperty("external_user_id")]
        public string ExternalUserId { get; set; }

        [JsonProperty("facebook_id")]
        public string FacebookId { get; set; }

        [JsonProperty("twitter_id")]
        public string TwitterId { get; set; }

        [JsonProperty("blob_id")]
        public object BlobId { get; set; }

        [JsonProperty("custom_data")]
        public object CustomData { get; set; }

        [JsonProperty("user_tags")]
        public string UserTags { get; set; }
    }
}
