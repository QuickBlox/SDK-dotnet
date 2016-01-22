using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace Quickblox.Sdk.Modules.ContentModule
{
    /// <summary>
    /// Information about Blob uploaded with Content module.
    /// </summary>
    public class BlobUploadInfo
    {
        public int Id { get; set; }
        public string UId { get; set; }
        public bool IsPublic { get; set; }
    }
}
