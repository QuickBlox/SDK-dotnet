using System;
using System.Runtime.Serialization;

namespace Quickblox.Sdk.Modules.AuthModule.Response
{
    [DataContract]
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

        [DataMember(Name = "account_id")]
        public Int32 AccountId { get; set; }

        [DataMember(Name = "api_endpoint")]
        public String ApiEndPoint { get; set; }

        [DataMember(Name = "chat_endpoint")]
        public String ChatEndPoint { get; set; }

        [DataMember(Name = "turnserver_endpoint")]
        public String TurnServerEndPoint { get; set; }

        [DataMember(Name = "s3_bucket_name")]
        public String S3BucketName { get; set; }

    }
}
