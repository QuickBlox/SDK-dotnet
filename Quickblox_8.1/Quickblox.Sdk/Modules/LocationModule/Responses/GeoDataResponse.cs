using Newtonsoft.Json;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Responses
{
    public class GeoDataResponse
    {
        [JsonProperty(PropertyName = "geo_data")]
        public InfoLocation GeoData { get; set; }
    }
}
