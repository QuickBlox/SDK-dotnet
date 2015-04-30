using System;
using System.Net;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.MessagesModule;
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk
{
    /// <summary>
    /// QuickbloxClient class.
    /// </summary>
    public class QuickbloxClient
    {
        private readonly String baseUri;
        private readonly String accountKey;
        private readonly ICryptographicProvider cryptographicProvider;
        private Boolean isClientInitialized;

        #region Ctor

        /// <summary>
        /// Инициализирует новый экземпляр класса QuickbloxClient.
        /// </summary>
        /// <param name="baseUri">Начальный урл.</param>
        /// <param name="accountKey">Ключ аккаунта</param>
        /// <param name="cryptographicProvider"></param>
        public QuickbloxClient(String baseUri, String accountKey, ICryptographicProvider cryptographicProvider)
        {
            this.baseUri = baseUri;
            this.accountKey = accountKey;
            this.CryptographicProvider = cryptographicProvider;

            this.CoreClient = new AuthorizationClient(this);
            this.ChatClient = new ChatClient(this);
            this.UsersClient = new UsersClient(this);
            this.NotificationClient = new NotificationClient(this);
            this.MessagesClient = new MessagesClient(this);
            this.ContentClient = new ContentClient(this);
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

        /// <summary>
        /// Возварщает время последнего запроса в UTC.
        /// </summary>
        public DateTime LastRequest
        {
            get { return HttpBase.LastRequest; }
        }

        public Boolean IsClientInitialized
        {
            get { return this.isClientInitialized; }
            private set
            {
                this.isClientInitialized = value;
                this.OnStatusChanged();
            }
        }

        public string ApiEndPoint { get; private set; }

        public string ChatEndpoint { get; private set; }

        public string Token { get; internal set; }

        #endregion

        #region Public Members
        
        public async Task InitializeClientAsync()
        {
            while (!this.IsClientInitialized)
            {
                await this.GetAccountSettingsAsync();
            }
        }

        #region Internal

        internal void CheckIsInitialized()
        {
            if (!this.IsClientInitialized)
                throw new NotInitializedException();
        }

        #endregion

        #endregion

        #region Private

        private async Task GetAccountSettingsAsync()
        {
                var accountResponse =
                    await HttpService.GetAsync<AccountResponse>(this.baseUri, QuickbloxMethods.AccountMethod,
                          RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbAccountKey(this.accountKey));
            if (accountResponse.StatusCode == HttpStatusCode.OK)
            {
                this.ApiEndPoint = accountResponse.Result.ApiEndPoint;
                this.ChatEndpoint = accountResponse.Result.ChatEndPoint;
                this.IsClientInitialized = true;
            }
        }

        private void OnStatusChanged()
        {
            var handler = this.ClientStatusChanged;
            if (handler != null)
            {
                handler.Invoke(this, this.isClientInitialized);
            }
        }

        #endregion
    }
}
