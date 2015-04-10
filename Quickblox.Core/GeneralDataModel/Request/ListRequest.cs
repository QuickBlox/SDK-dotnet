using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Request
{
    public class ListRequest : BaseRequestSettings
    {
        /// <summary>
        /// Limit search results to N records. Useful for pagination. Default value - 100.
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        /// <summary>
        /// Skip N records in search results. Useful for pagination. Default (if not specified): 0.
        /// </summary>
        [JsonProperty("skip", NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }

        /// <summary>
        /// Count search results. Response will contain only count of records found.
        /// </summary>
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
