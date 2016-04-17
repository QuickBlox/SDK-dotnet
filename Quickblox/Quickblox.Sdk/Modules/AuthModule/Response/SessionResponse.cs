using Newtonsoft.Json;
using Quickblox.Sdk.Modules.AuthModule.Models;

namespace Quickblox.Sdk.Modules.AuthModule.Response
{
    public class SessionResponse
    {
        [JsonProperty("session")]
        public Session Session { get; set; }
    }
}
