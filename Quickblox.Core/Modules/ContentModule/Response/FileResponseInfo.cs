using Newtonsoft.Json;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Response
{
    public class FileResponseInfo
    {
        [JsonProperty("blob")]
        public BlobRequest Blob { get; set; }
    }
}
