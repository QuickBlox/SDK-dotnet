using System;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Requests
{
    public class UploadFileRequest : BaseRequestSettings
    {
        public BlobObjectAccess BlobObjectAccess { get; set; }

        public BytesContent FileContent { get; set; }
    }
}
