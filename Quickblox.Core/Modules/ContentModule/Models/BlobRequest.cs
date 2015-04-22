using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ContentModule.Models
{
    public class BlobRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobRequest"/> class.
        /// </summary>
        public BlobRequest()
        {
            ContentType = "image/jpeg";
        }

        /// <summary>
        /// Mime content type.
        /// </summary>
        [JsonProperty(PropertyName = "content_type")]
        public String ContentType { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        /// <value>
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        /// <summary>
        /// Blob visibility (by default: false)
        /// </summary>
        [JsonProperty(PropertyName = "public", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? IsPublic { get; set; }

        /// <summary>
        /// Should be a coma separated string with tags
        /// </summary>
        [JsonProperty(PropertyName = "tag_list", NullValueHandling = NullValueHandling.Ignore)]
        public String TagList { get; set; }

        /// <summary>
        /// Use it to update blob file.
        /// </summary>
        [JsonProperty(PropertyName = "new", NullValueHandling = NullValueHandling.Ignore)]
        public Int32? NewId { get; set; }
    }
}
