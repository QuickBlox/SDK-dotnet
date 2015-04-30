using Newtonsoft.Json;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Response
{
    public class FileInfoResponse
    {
        [JsonProperty("blob")]
        public Blob Blob { get; set; }
    }
}
