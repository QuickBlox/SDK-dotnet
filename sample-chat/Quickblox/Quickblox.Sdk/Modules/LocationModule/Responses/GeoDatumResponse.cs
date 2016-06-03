using Newtonsoft.Json;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Responses
{
    public class GeoDatumResponse
    {
        [JsonProperty(PropertyName = "geo_datum")]
        public InfoLocation GeoDatum { get; set; }
    }
}
