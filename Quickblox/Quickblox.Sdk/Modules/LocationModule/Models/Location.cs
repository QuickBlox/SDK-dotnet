using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.LocationModule.Models
{
    public class Location
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longtitude { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
