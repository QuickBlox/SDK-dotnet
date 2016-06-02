using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ContentModule.Models
{
    public class Blob
    {
        [JsonProperty("blob_status")]
        public object BlobStatus { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("last_read_access_ts")]
        public object LastReadAccessTs { get; set; }

        [JsonProperty("lifetime")]
        public int Lifetime { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("ref_count")]
        public int RefCount { get; set; }

        [JsonProperty("set_completed_at")]
        public object SetCompletedAt { get; set; }

        [JsonProperty("size")]
        public object Size { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("blob_object_access")]
        public BlobObjectAccess BlobObjectAccess { get; set; }
    }
}
