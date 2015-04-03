using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{

    public class User
    {
        [JsonProperty("ids")]
        public string Ids { get; set; }
    }

}
