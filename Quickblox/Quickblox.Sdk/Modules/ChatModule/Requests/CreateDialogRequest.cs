using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.ChatModule.Requests
{
    public class CreateDialogRequest : BaseRequestSettings
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("occupants_ids", NullValueHandling=NullValueHandling.Ignore)]
        public string OccupantsIds { get; set; }

        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public string Photo { get; set; }
    }
}
