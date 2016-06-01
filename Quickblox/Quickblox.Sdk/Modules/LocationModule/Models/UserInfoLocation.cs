using Newtonsoft.Json;
using Quickblox.Sdk.Modules.AuthModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Models
{
    public class UserInfoLocation : InfoLocation
    {
        [JsonProperty(PropertyName = "user")]
        public FullUser User { get; set; }
    }
}
