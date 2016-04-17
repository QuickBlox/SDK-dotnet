using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Requests
{
    public class CreateGeoDataRequest : BaseRequestSettings
    {
        [JsonProperty(PropertyName = "geo_data")]
        public Location GeoData { get; set; }
    }
}
