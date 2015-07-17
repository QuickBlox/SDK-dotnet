using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.LocationModule.Models
{
    public class InfoLocation : Location
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "application_id")]
        public int ApplicationId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty(PropertyName = "created_at_timestamp")]
        public TimeSpan CreatedAtTimeSpan { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
