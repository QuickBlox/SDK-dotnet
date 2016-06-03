using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.AuthModule.Models
{
    public class User
    {
        [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
        public string Login { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
