using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.CoreModule.Models
{
    public class Device
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("udid")]
        public string Udid { get; set; }
    }
}
