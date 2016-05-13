using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.CustomObjectModule;
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;
using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Logger;
using Quickblox.Sdk.Modules.LocationModule;
using Quickblox.Sdk.Modules.ChatXmppModule;
using Quickblox.Sdk.Platform;

namespace Quickblox.Sdk
{
    /// <summary>
    /// QuickbloxClient class. Primary class in this SDK.
    /// </summary>
    public class QuickbloxClient
    {
        private const string defaultApiEndpoint = "https://api.quickblox.com";
        private const string defaultChatEndpoint = "chat.quickblox.com";

        #region Ctor

        ///// <summary>
        ///// QuickbloxClient ctor.
        ///// </summary>
        ///// <param name="applicationId">Quickblox application ID</param>
        ///// <param name="authKey">Auth Key</param>
        ///// <param name="authSecret">Auth Secret</param>
        ///// <param name="logger">Logger instance. Allows to log API calls, xmpp messages etc.</param>
        //public QuickbloxClient(int applicationId, string authKey, string authSecret, ILogger logger = null)
        //    : this(applicationId, authKey, authSecret, defaultApiEndpoint, defaultChatEndpoint, logger)
        //{

        //}

        ///// <summary>
        ///// QuickbloxClient ctor.
        ///// </summary>
        ///// <param name="applicationId">Quickblox application ID</param>
        ///// <param name="authKey">Auth Key</param>
        ///// <param name="authSecret">Auth Secret</param>
        ///// <param name="apiEndpoint">API endpoint</param>
        ///// <param name="chatEndpoint">XMPP chat endpoint</param>
        ///// <param name="logger">Logger instance. Allows to log API calls, xmpp messages etc.</param>
        public QuickbloxClient(int applicationId, string authKey, string authSecret, string apiEndpoint, string chatEndpoint, ILogger logger = null)
            : this(applicationId, authKey, authSecret, apiEndpoint, chatEndpoint, new HmacSha1CryptographicProvider(), logger)
        {

        }

        /// <summary>
        /// QuickbloxClient ctor.
        /// </summary>
        /// <param name="applicationId">Quickblox application ID</param>
        /// <param name="authKey">Auth Key</param>
        /// <param name="authSecret">Auth Secret</param>
        /// <param name="apiEndpoint">API endpoint</param>
        /// <param name="chatEndpoint">XMPP chat endpoint</param>
        /// <param name="cryptographicProvider">HMAC SHA1 Cryptographic Provider</param>
        /// <param name="logger">Logger instance. Allows to log API calls, xmpp messages etc.</param>
        public QuickbloxClient(int applicationId, string authKey, string authSecret, string apiEndpoint, string chatEndpoint, ICryptographicProvider cryptographicProvider, ILogger logger = null)
        {
            if (apiEndpoint == null) throw new ArgumentNullException(nameof(apiEndpoint));
            if (chatEndpoint == null) throw new ArgumentNullException(nameof(chatEndpoint));
            if (authKey == null) throw new ArgumentNullException(nameof(authKey));
            if (authSecret == null) throw new ArgumentNullException(nameof(authSecret));
            if (cryptographicProvider == null) throw new ArgumentNullException(nameof(cryptographicProvider));

            if (logger != null)
            {
                LoggerHolder.LoggerInstance = logger;
            }

            ApplicationId = applicationId;
            AuthKey = authKey;
            AuthSecret = authSecret;
            ApiEndPoint = apiEndpoint;
            ChatEndpoint = chatEndpoint;

            this.AuthenticationClient = new AuthenticationClient(this, cryptographicProvider);
            this.ChatClient = new ChatClient(this);
            this.UsersClient = new UsersClient(this);
            this.NotificationClient = new NotificationClient(this);
            this.LocationClient = new LocationClient(this);
            this.ChatXmppClient = new ChatXmppClient(this);
            this.WebSyncClient = new WebSyncClient(this);
            this.ContentClient = new ContentClient(this);      
            this.CustomObjectsClient = new CustomObjectsClient(this);
        }

        #endregion

        #region Properties

        #region Clients

        /// <summary>
        /// Content module allows to manage app contents and settings.
        /// </summary>
        public ContentClient ContentClient { get; private set; }

        /// <summary>
        /// Authentication module allows to manage user sessions.
        /// </summary>
        public AuthenticationClient AuthenticationClient { get; private set; }

        /// <summary>
        /// Chat module allows to manage user dialogs.
        /// </summary>
        public ChatClient ChatClient { get; private set; }

        /// <summary>
        /// User module manages all things related to user accounts handling, authentication, account data, password reminding etc.
        /// </summary>
        public UsersClient UsersClient { get; private set; }

        /// <summary>
        /// Notification module allows to manage push and email notifications to users.
        /// </summary>
        public NotificationClient NotificationClient { get; private set; }

        /// <summary>
        /// Location module allows to work with user locations.
        /// </summary>
        public LocationClient LocationClient { get; private set; }

        /// <summary>
        /// ChatXmpp module allows users to chat with each other in private or group dialogs via XMPP protocol.
        /// </summary>
        public ChatXmppClient ChatXmppClient { get; private set; }

        public WebSyncClient WebSyncClient { get; private set; }

        /// <summary>
        /// Custom Objects module provides flexibility to define any data structure(schema) you need.
        /// Schema is defined in QuickBlox Administration Panel. The schema is called Class and contains field names and their type.
        /// </summary>
        public CustomObjectsClient CustomObjectsClient { get; private set; }

        #endregion

        /// <summary>
        /// Quickblox aplication ID.
        /// </summary>
        public int ApplicationId { get; private set; }

        /// <summary>
        /// Authorization Key
        /// </summary>
        public string AuthKey { get; private set; }

        /// <summary>
        /// Authorization Secret
        /// </summary>
        public string AuthSecret { get; private set; }

        /// <summary>
        /// API endpoint
        /// </summary>
        public string ApiEndPoint { get; private set; }
        
        /// <summary>
        /// XMPP Chat endpoint
        /// </summary>
        public string ChatEndpoint { get; private set; }

        /// <summary>
        /// Group chats XMPP endpoint.
        /// </summary>
        public string MucChatEndpoint
        {
            get
            {
                return string.Format("muc.{0}", ChatEndpoint);
            }
        }

        /// <summary>
        /// Quickblox token. Must be set before calling any methods that require authentication.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// UTC DateTime of the last request to the server.
        /// </summary>
        public DateTime LastRequest
        {
            get { return HttpBase.LastRequest; }
        }
        
        #endregion

        #region Public Members

        /// <summary>
        /// Returns account settings (account ID, endpoints, etc.)
        /// </summary>
        /// <param name="accountKey">Account key from admin panel</param>
        /// <returns>AccountResponse</returns>
        public async Task<HttpResponse<AccountResponse>> GetAccountSettingsAsync(string accountKey)
        {
            if (accountKey == null) throw new ArgumentNullException("accountKey");

            var accountResponse = await HttpService.GetAsync<AccountResponse>(ApiEndPoint, QuickbloxMethods.AccountMethod,
                      RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbAccountKey(accountKey));

            return accountResponse;
        }

        //public void Resume(string token)
        //{
        //    Token = token;
        //}

        //public string Suspend()
        //{
        //    return Token;
        //}

#endregion
    }
}
