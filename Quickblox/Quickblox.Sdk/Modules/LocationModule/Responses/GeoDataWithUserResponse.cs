using Newtonsoft.Json;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Responses
{
    public class GeoDataWithUserResponse
    {
        [JsonProperty(PropertyName = "geo_data")]
        public UserInfoLocation GeoData { get; set; }
    }
}
