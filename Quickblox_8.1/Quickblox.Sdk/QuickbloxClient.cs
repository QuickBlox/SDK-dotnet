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

namespace Quickblox.Sdk
{
    /// <summary>
    /// QuickbloxClient class.
    /// </summary>
    public class QuickbloxClient
    {
        private String baseUri;
        private String accountKey;
        private ICryptographicProvider cryptographicProvider;
        private string chatUri;

        #region Ctor

        public QuickbloxClient()
        {
            this.CoreClient = new AuthorizationClient(this);
            this.ChatClient = new ChatClient(this);
            this.UsersClient = new UsersClient(this);
            this.NotificationClient = new NotificationClient(this);
            this.MessagesClient = new MessagesClient(this);
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

        public MessagesClient MessagesClient { get; private set; }

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

        public string Token { get; internal set; }

        #endregion

        #region Public Members
        
        public async Task InitializeClientAsync(String baseUri, String accountKey, ICryptographicProvider cryptographicProvider)
        {
            if (baseUri == null) throw new ArgumentNullException("baseUri");
            if (accountKey == null) throw new ArgumentNullException("accountKey");
            if (cryptographicProvider == null) throw new ArgumentNullException("cryptographicProvider");
            this.baseUri = baseUri;
            this.accountKey = accountKey;
            this.CryptographicProvider = cryptographicProvider;
            await this.GetAccountSettingsAsync();
        }

        public void Resume(string token)
        {
            Token = token;
        }

        public string Suspend()
        {
            return Token;
        }

        #endregion

        #region Private

        private async Task GetAccountSettingsAsync()
        {
                var accountResponse =
                    await HttpService.GetAsync<AccountResponse>(this.baseUri, QuickbloxMethods.AccountMethod,
                          RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbAccountKey(this.accountKey));

                this.ApiEndPoint = accountResponse.Result != null ? accountResponse.Result.ApiEndPoint : this.baseUri;
                this.ChatEndpoint = accountResponse.Result != null ? accountResponse.Result.ChatEndPoint : this.chatUri;
        }

        #endregion
    }
}
