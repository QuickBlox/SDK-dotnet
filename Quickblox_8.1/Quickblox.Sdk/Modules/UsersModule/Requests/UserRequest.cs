using System;
using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.UsersModule.Requests
{
    /// <summary>
    /// Used for updating user information.
    /// </summary>
    public class UserRequest : PagedRequestSettings
    {
        /// <summary>
        /// API User login.
        /// </summary>
        [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
        public String Login { get; set; }

        /// <summary>
        /// API User password.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public String Password { get; set; }

        /// <summary>
        /// API User e-mail.
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public String Email { get; set; }

        /// <summary>
        /// ID of associated blob (for example, API User photo).
        /// </summary>
        [JsonProperty("blob_id", NullValueHandling = NullValueHandling.Ignore)]
        public Int32? BlobId { get; set; }

        /// <summary>
        /// ID of API User in external system.
        /// </summary>
        [JsonProperty("external_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public Int32? ExternalUserId { get; set; }

        /// <summary>
        /// ID of API User in Facebook.
        /// </summary>
        [JsonProperty("facebook_id", NullValueHandling = NullValueHandling.Ignore)]
        public Int64? FacebookId { get; set; }

        /// <summary>
        /// ID of API User in Twitter.
        /// </summary>
        [JsonProperty("twitter_id", NullValueHandling = NullValueHandling.Ignore)]
        public Int32? TwitterId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [JsonProperty("full_name", NullValueHandling = NullValueHandling.Ignore)]
        public String FullName { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public String Phone { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public String Website { get; set; }

        /// <summary>
        /// Gets or sets the tag list.
        /// </summary>
        [JsonProperty("tag_list", NullValueHandling = NullValueHandling.Ignore)]
        public String TagList { get; set; }

        /// <summary>
        /// User's additional info.
        /// </summary>
        [JsonProperty("custom_data", NullValueHandling = NullValueHandling.Ignore)]
        public String CustomData { get; set; }

        /// <summary>
        /// Old user password (required only if new password provided)
        /// </summary>
        [JsonProperty("old_password", NullValueHandling = NullValueHandling.Ignore)]
        public string OldPassword { get; set; }
    }
}
