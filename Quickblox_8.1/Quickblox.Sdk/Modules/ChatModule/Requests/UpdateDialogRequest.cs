using System.Collections.Generic;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ChatModule.Requests
{
    public class UpdateDialogRequest
    {
        [JsonIgnore()]
        public string DialogId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Occupants to be removed
        /// </summary>
        [JsonProperty("pull_all", NullValueHandling = NullValueHandling.Ignore)]
        public EditedOccupants PullAll { get; set; }

        /// <summary>
        /// Occupants to be added
        /// </summary>
        [JsonProperty("push_all", NullValueHandling = NullValueHandling.Ignore)]
        public EditedOccupants PushAll { get; set; }


    }

    public class EditedOccupants
    {
        [JsonProperty("occupants_ids")]
        public IList<int> OccupantsIds { get; set; }
    }
}
