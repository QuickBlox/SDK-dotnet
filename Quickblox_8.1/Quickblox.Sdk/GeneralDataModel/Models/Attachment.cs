using System.Xml.Linq;
using Newtonsoft.Json;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}