using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.AuthModule.Models;

namespace Quickblox.Sdk.Modules.AuthModule.Requests
{
    public class SessionRequest : BaseRequestSettings
    {
        public SessionRequest()
        {
            this.Headers = new Dictionary<string, string>();
        }

        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }

        [JsonProperty("auth_key")]
        public string AuthKey { get; set; }

        [JsonProperty("timestamp")]
        public Int64 Timestamp { get; set; }

        [JsonProperty("nonce")]
        public Int32 Nonce { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }

        [JsonProperty("device", NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public String Provider { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public SocialScope? Scope { get; set; }

        [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
        public SocialNetworkKey SocialNetworkKey { get; set; }
    }
}
