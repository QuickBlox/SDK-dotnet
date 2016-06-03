using Newtonsoft.Json;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Responses
{
    public class GeoDatumWithUserResponse
    {
        [JsonProperty(PropertyName = "geo_datum")]
        public UserInfoLocation GeoData { get; set; }
    }
}
