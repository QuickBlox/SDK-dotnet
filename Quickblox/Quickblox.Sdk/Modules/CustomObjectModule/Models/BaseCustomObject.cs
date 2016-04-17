using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Models
{
    public class BaseCustomObject
    {
        [JsonProperty(PropertyName = "_id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public Int64 CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public Int64 UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public Int64 UserId { get; set; }

        [JsonProperty(PropertyName = "_parent_id")]
        public String ParentId { get; set; }

        [JsonProperty(PropertyName = "permissions", NullValueHandling = NullValueHandling.Ignore)]
        public Permissions Permissions { get; set; }
    }
}
