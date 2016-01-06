using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.UsersModule.Models
{
    public class CustomData
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("is_import")]
        public bool IsImport { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
