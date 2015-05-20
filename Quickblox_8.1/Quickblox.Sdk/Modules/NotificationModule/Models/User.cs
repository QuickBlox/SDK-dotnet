using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class User
    {
        /// <summary>
        /// Should contain a string of external users' ids divided by commas.
        /// </summary>
        [JsonProperty("ids")]
        public string Ids { get; set; }
    }
}
