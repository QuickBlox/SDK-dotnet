using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.AuthModule.Response
{
    public class AccountResponse 
    {
        public AccountResponse()
        {
            //"account_id":26595,
            //"api_endpoint":"https://api.quickblox.com",
            //"chat_endpoint":"chat.quickblox.com",
            //"turnserver_endpoint":"turnserver.quickblox.com",
            //"s3_bucket_name":"qbprod"
        }

        [JsonProperty(PropertyName = "account_id")]
        public Int32 AccountId { get; set; }

        [JsonProperty(PropertyName = "api_endpoint")]
        public String ApiEndPoint { get; set; }

        [JsonProperty(PropertyName = "chat_endpoint")]
        public String ChatEndPoint { get; set; }

        [JsonProperty(PropertyName = "turnserver_endpoint")]
        public String TurnServerEndPoint { get; set; }

        [JsonProperty(PropertyName = "s3_bucket_name")]
        public String S3BucketName { get; set; }

    }
}
