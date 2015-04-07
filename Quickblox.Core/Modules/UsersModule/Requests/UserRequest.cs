using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.UsersModule.Requests
{
    /// <summary>
    /// User to be used in User sign up request.
    /// </summary>
    public class UserRequest : BaseRequestSettings
    {
        /// <summary>
        /// API User login.
        /// </summary>
        [JsonProperty("login")]
        public String Login { get; set; }

        /// <summary>
        /// API User password.
        /// </summary>
        [JsonProperty("password")]
        public String Password { get; set; }

        /// <summary>
        /// API User e-mail.
        /// </summary>
        [JsonProperty("email")]
        public String Email { get; set; }

        /// <summary>
        /// ID of associated blob (for example, API User photo).
        /// </summary>
        [JsonProperty("blob_id")]
        public Int32? BlobId { get; set; }

        /// <summary>
        /// ID of API User in external system.
        /// </summary>
        [JsonProperty("external_user_id")]
        public Int32? ExternalUserId { get; set; }

        /// <summary>
        /// ID of API User in Facebook.
        /// </summary>
        [JsonProperty("facebook_id")]
        public Int32? FacebookId { get; set; }

        /// <summary>
        /// ID of API User in Twitter.
        /// </summary>
        [JsonProperty("twitter_id")]
        public Int32? TwitterId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [JsonProperty("full_name")]
        public String FullName { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        [JsonProperty("phone")]
        public String Phone { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [JsonProperty("website")]
        public String Website { get; set; }

        /// <summary>
        /// Gets or sets the tag list.
        /// </summary>
        [JsonProperty("tag_list")]
        public String TagList { get; set; }

        /// <summary>
        /// User's additional info.
        /// </summary>
        [JsonProperty("custom_data")]
        public String CustomData { get; set; }

        /// <summary>
        /// page No  Unsigned Integer	3	Page number of the book of the results that you want to get.By default: 1
        /// </summary>
        [JsonProperty("page")]
        public UInt32? Page { get; set; }

        /// <summary>
        /// per_page No  Unsigned Integer	15	The maximum number of results per page.Min: 1. Max: 100. By default: 10 
        /// </summary>
        [JsonProperty("per_page")]
        public UInt32? PerPage { get; set; }

    }
}
