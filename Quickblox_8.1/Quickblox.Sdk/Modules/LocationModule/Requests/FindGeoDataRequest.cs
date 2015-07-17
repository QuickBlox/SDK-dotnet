using System;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.LocationModule.Models;

namespace Quickblox.Sdk.Modules.LocationModule.Requests
{
    /// <summary>
    /// http://quickblox.com/developers/Location#Retrieve_geodata_by_the_identifier
    /// </summary>
    public class FindGeoDataRequest : BaseRequestSettings
    {
        #region Filters

        [JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan CreatedAt { get; set; }

        [JsonProperty(PropertyName = "user.id", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 UserId { get; set; }

        [JsonProperty(PropertyName = "user.ids", NullValueHandling = NullValueHandling.Ignore)]
        public String UserIds { get; set; }

        [JsonProperty(PropertyName = "user.name", NullValueHandling = NullValueHandling.Ignore)]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "user.external_ids", NullValueHandling = NullValueHandling.Ignore)]
        public String ExternaIds { get; set; }

        #endregion

        #region Diapasons

        [JsonProperty(PropertyName = "min_created_at", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan MinCreatedAt { get; set; }

        [JsonProperty(PropertyName = "max_created_at", NullValueHandling = NullValueHandling.Ignore)]
        public TimeSpan MaxCreatedAt { get; set; }

        [JsonProperty(PropertyName = "geo_rect", NullValueHandling = NullValueHandling.Ignore)]
        public GeoRect GeoRect { get; set; }

        [JsonProperty(PropertyName = "radius", NullValueHandling = NullValueHandling.Ignore)]
        public Int32 Radius { get; set; }

        #endregion

        #region Sort
        // <summary>
        // 1 (all other values ​​cause an error validation)
        // </summary>

        /// <summary>
        /// created_at	Values ​​should be sorted by date.
        /// latitude Values ​​should be sorted by latitude.
        /// longitude Values ​​should be sorted by longitude.
        /// distance Values ​​should be sorted by the distance from the 'current_position' (is a required parameter in the request)
        /// </summary>
        [JsonProperty(PropertyName = "sort_by", NullValueHandling = NullValueHandling.Ignore)]
        public String SortBy { get; set; }
        
        #endregion

        #region Paginal conclusion

        [JsonProperty(PropertyName = "page", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32 Page { get; set; }

        [JsonProperty(PropertyName = "per_page", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32 PerPage { get; set; }

        #endregion

        #region Additional keys

        // <summary>
        // 1 (all other values ​​cause an error validation)
        // </summary>

        [JsonProperty(PropertyName = "last_only", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32 IsLastOnly { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32 IsOnlyWithStatus { get; set; }
        
        [JsonProperty(PropertyName = "sort_asc", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32 SortAsc { get; set; }

        /// <summary>
        /// The current position of the user. Used only in conjunction with the keys 'radius' (Diapasons) and 'distance' (Sort). 
        /// If this option is specified, and it does not set any of these parameters - error validation. 
        /// Use '%3B' instead ';'.
        /// Sample format:90%3B90 (90;90)
        /// </summary>
        [JsonProperty(PropertyName = "current_position", NullValueHandling = NullValueHandling.Ignore)]
        public String CurrentPosition { get; set; }

        #endregion
    }
}
