using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Requests
{
    /// <summary>
    /// Create an entity which is a file in a system
    /// </summary>
    public class CreateFileRequest : BaseRequestSettings
    {
        [JsonProperty(PropertyName = "blob")]
        public BlobRequest Blob { get; set; }
    }
}
