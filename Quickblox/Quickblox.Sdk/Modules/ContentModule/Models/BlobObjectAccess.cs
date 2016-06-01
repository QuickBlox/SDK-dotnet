using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ContentModule.Models
{
    public class BlobObjectAccess 
    {
        [JsonProperty("blob_id")]
        public int BlobId { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("object_access_type")]
        public string ObjectAccessType { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }
    }
}
