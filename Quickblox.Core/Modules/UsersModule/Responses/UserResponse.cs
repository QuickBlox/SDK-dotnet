using Newtonsoft.Json;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace Quickblox.Sdk.Modules.UsersModule.Responses
{
    public class UserResponse
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
