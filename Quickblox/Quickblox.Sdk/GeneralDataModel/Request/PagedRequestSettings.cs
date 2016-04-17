using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Request
{
    public abstract class PagedRequestSettings : BaseRequestSettings
    {
        /// <summary>
        /// page No  Unsigned Integer	3	Page number of the book of the results that you want to get.By default: 1
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32? Page { get; set; }

        /// <summary>
        /// per_page No  Unsigned Integer	15	The maximum number of results per page.Min: 1. Max: 100. By default: 10 
        /// </summary>
        [JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        public UInt32? PerPage { get; set; }
    }
}
