using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class PagedResponse<T>
    {
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total_entries")]
        public int TotalEntries { get; set; }

        [JsonProperty("items")]
        public T[] Items { get; set; }
    }
}
