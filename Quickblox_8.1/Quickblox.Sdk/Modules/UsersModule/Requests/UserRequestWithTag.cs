using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using System;

namespace Quickblox.Sdk.Modules.UsersModule.Requests
{
    public class UserRequestWithTag : PagedRequestSettings
    {
        /// <summary>
        /// API User login.
        /// </summary>
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public String Tags { get; set; }
    }
}
