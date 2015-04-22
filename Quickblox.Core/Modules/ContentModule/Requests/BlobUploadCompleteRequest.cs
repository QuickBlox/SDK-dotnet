using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Requests
{
    public class BlobUploadCompleteRequest : BaseRequestSettings
    {
        [JsonProperty(PropertyName = "blob")]
        public BlobUploadSize BlobUploadSize { get; set; }
    }
}
