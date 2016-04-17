using System;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.AuthModule.Models;

namespace Quickblox.Sdk.Modules.AuthModule.Requests
{
    using Newtonsoft.Json;

    public class LoginRequest : BaseRequestSettings
    {
        [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
        public String Login { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public String Email { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public String Provider { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public String Scope { get; set; }

        [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
        public SocialNetworkKey SocialNetworkKey { get; set; }
    }
}
