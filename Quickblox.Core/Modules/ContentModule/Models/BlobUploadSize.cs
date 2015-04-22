using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ContentModule.Models
{
    public class BlobUploadSize
    {
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
    }
}
