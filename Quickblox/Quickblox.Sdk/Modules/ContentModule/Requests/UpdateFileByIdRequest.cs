using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Requests
{
    public class UpdateFileByIdRequest : BaseRequestSettings
    {
        public BlobRequest BlobRequest { get; set; }
    }
}