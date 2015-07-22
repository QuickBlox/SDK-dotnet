using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.CustomObjectModule;
#if !Xamarin
using Quickblox.Sdk.Modules.MessagesModule;
#endif
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;
using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.AuthModule.Models;
using Quickblox.Sdk.Modules.LocationModule;

namespace Quickblox.Sdk
{
    /// <summary>
    /// QuickbloxClient class.
    /// </summary>
    public class QuickbloxClient : IQuickbloxClient
    {
#region Ctor

        public QuickbloxClient(string apiEndpoint, string chatEndpoint, ICryptographicProvider cryptographicProvider)
        {
            if (apiEndpoint == null) throw new ArgumentNullException("apiEndpoint");
            if (chatEndpoint == null) throw new ArgumentNullException("chatEndpoint");
            if (cryptographicProvider == null) throw new ArgumentNullException("cryptographicProvider");

            ApiEndPoint = apiEndpoint;
            ChatEndpoint = chatEndpoint;
            CryptographicProvider = cryptographicProvider;

            this.CoreClient = new AuthorizationClient(this);
            this.ChatClient = new ChatClient(this);
            this.UsersClient = new UsersClient(this);
            this.NotificationClient = new NotificationClient(this);
            this.LocationClient = new LocationClient(this);
#if !Xamarin
            this.MessagesClient = new MessagesClient(this);
#endif
            this.ContentClient = new ContentClient(this);      
            this.CustomObjectsClient = new CustomObjectsClient(this);
        }

#endregion

#region Events

        public event EventHandler<Boolean> ClientStatusChanged;

#endregion

#region Properties

        public ICryptographicProvider CryptographicProvider { get; private set; }

        public ContentClient ContentClient { get; private set; }
        
        public AuthorizationClient CoreClient { get; private set; }

        public ChatClient ChatClient { get; private set; }

        public UsersClient UsersClient { get; private set; }

        public NotificationClient NotificationClient { get; private set; }

        public LocationClient LocationClient { get; private set; }
#if !Xamarin
        public MessagesClient MessagesClient { get; private set; }
#endif
        public CustomObjectsClient CustomObjectsClient { get; private set; }

        /// <summary>
        /// Возварщает время последнего запроса в UTC.
        /// </summary>
        public DateTime LastRequest
        {
            get { return HttpBase.LastRequest; }
        }

        public string ApiEndPoint { get; private set; }

        public string ChatEndpoint { get; private set; }

        public string Token { get; set; }

        public int CurrentUserId { get; set; }

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
