using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ChatModule.Requests
{
    public class UpdateDialogRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("occupants_ids")]
        public string OccupantsIds { get; set; }
    }
}
