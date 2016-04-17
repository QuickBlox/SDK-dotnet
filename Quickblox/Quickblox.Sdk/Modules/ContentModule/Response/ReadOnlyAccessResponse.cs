using Newtonsoft.Json;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Response
{
    public class ReadOnlyAccessResponse
    {
        [JsonProperty(PropertyName = "blob_object_access")]
        public BlobObjectAccess BlobObjectAccess { get; set; }
    }
}
