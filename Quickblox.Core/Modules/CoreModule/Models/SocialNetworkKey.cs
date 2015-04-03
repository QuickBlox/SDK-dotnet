namespace Quickblox.Sdk.Modules.CoreModule.Models
{
    using System;
    using Newtonsoft.Json;
    
    public class SocialNetworkKey
    {
        [JsonProperty("token")]
        public String Token { get; set; }

        [JsonProperty("secret")]
        public String Secret { get; set; }
    }
}
