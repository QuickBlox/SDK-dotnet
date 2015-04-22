using System;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.ContentModule.Models;

namespace Quickblox.Sdk.Modules.ContentModule.Requests
{
    public class UploadFileRequest : BaseRequestSettings
    {
        public Blob FileInfo { get; set; }

        public Byte[] FileContent { get; set; }
    }
}
