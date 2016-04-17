namespace Quickblox.Sdk.Modules.AuthModule.Models
{
    using System;
    using Newtonsoft.Json;
    
    public class SocialNetworkKey
    {
        [JsonProperty("token")]
        public String Token { get; set; }

        [JsonProperty("secret", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Secret { get; set; }
    }
}
