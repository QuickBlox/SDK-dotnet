using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Response
{
    public class ListResponse<T>
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("skip")]
        public int Skip { get; set; }

        [JsonProperty("items")]
        public T[] Items { get; set; }
    }
}
