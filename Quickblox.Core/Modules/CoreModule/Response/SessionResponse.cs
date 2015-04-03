using Newtonsoft.Json;
using Quickblox.Sdk.Modules.CoreModule.Models;

namespace Quickblox.Sdk.Modules.CoreModule.Response
{
    public class SessionResponse
    {
        [JsonProperty("session")]
        public Session Session { get; set; }
    }
}
