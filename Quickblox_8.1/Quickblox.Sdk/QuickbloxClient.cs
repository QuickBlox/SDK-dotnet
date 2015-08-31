using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.CustomObjectModule;
using Quickblox.Sdk.Modules.MessagesModule;
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;
using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.LocationModule;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;

namespace Quickblox.Sdk
{
    /// <summary>
    /// QuickbloxClient class.
    /// </summary>
    public class QuickbloxClient : IQuickbloxClient
    {
#region Ctor

        public QuickbloxClient(string apiEndpoint, string chatEndpoint)
            : this(apiEndpoint, chatEndpoint, new HmacSha1CryptographicProvider())
        {
            
        }

        public QuickbloxClient(string apiEndpoint, string chatEndpoint, ICryptographicProvider cryptographicProvider)
        {
            if (apiEndpoint == null) throw new ArgumentNullException("apiEndpoint");
            if (chatEndpoint == null) throw new ArgumentNullException("chatEndpoint");
            if (cryptographicProvider == null) throw new ArgumentNullException("cryptographicProvider");

            ApiEndPoint = apiEndpoint;
            ChatEndpoint = chatEndpoint;

            this.CoreClient = new AuthorizationClient(this, cryptographicProvider);
            this.ChatClient = new ChatClient(this);
            this.UsersClient = new UsersClient(this);
            this.NotificationClient = new NotificationClient(this);
            this.LocationClient = new LocationClient(this);
            this.MessagesClient = new MessagesClient(this);
            this.ContentClient = new ContentClient(this);      
            this.CustomObjectsClient = new CustomObjectsClient(this);
        }

#endregion

#region Properties

        /// <summary>
        /// The content module allows to manage app contents and settings.
        /// </summary>
        public ContentClient ContentClient { get; private set; }
        
        /// <summary>
        /// The authorization module allows to manage users' sessions.
        /// </summary>
        public AuthorizationClient CoreClient { get; private set; }

        /// <summary>
        /// The chat module allows to manage users' dialogs.
        /// </summary>
        public ChatClient ChatClient { get; private set; }

        /// <summary>
        /// The user module manages all things related to user accounts handling, authentication, account data, password reminding etc.
        /// </summary>
        public UsersClient UsersClient { get; private set; }

        /// <summary>
        /// The notification module allows to manage push and email notifications to users.
        /// </summary>
        public NotificationClient NotificationClient { get; private set; }

        /// <summary>
        /// The location module allows to work with users' locations.
        /// </summary>
        public LocationClient LocationClient { get; private set; }

        /// <summary>
        /// The messages module allows users to chat with each other in private or group dialogs via XMPP protocol.
        /// </summary>
        public IMessagesClient MessagesClient { get; private set; }

        /// <summary>
        /// Custom Objects module provides flexibility to define any data structure(schema) you need.
        /// Schema is defined in QuickBlox Administration Panel. The schema is called Class and contains field names and their type.
        /// </summary>
        public CustomObjectsClient CustomObjectsClient { get; private set; }

        /// <summary>
        /// UTC DateTime of the last request to the server.
        /// </summary>
        public DateTime LastRequest
        {
            get { return HttpBase.LastRequest; }
        }

        /// <summary>
        /// API endpoint
        /// </summary>
        public string ApiEndPoint { get; private set; }

        /// <summary>
        /// Chat endpoint
        /// </summary>
        public string ChatEndpoint { get; private set; }

        /// <summary>
        /// Quickblox token. Must be set before calling any methods that require authentication.
        /// </summary>
        public string Token { get; set; }

#endregion

#region Public Members

        public async Task GetAccountSettingsAsync(string accountKey)
        {
            if (accountKey == null) throw new ArgumentNullException("accountKey");

            var accountResponse = await HttpService.GetAsync<AccountResponse>(ApiEndPoint, QuickbloxMethods.AccountMethod,
                      RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbAccountKey(accountKey));

            if (accountResponse.Result != null)
            {
                this.ApiEndPoint = accountResponse.Result.ApiEndPoint;
                this.ChatEndpoint = accountResponse.Result.ChatEndPoint;
            }
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
