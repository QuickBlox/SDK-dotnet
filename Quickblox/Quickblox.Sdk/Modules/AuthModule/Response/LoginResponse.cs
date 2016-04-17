using Newtonsoft.Json;
using Quickblox.Sdk.Modules.AuthModule.Models;

namespace Quickblox.Sdk.Modules.AuthModule.Response
{
    public class LoginResponse
    {
        [JsonProperty("user")]
        public FullUser User { get; set; }
    }
}
