using System;
using System.Linq;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Modules.AuthModule.Requests;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.AuthModule.Models;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk.Modules.AuthModule
{
    /// <summary>
    /// Authorization module allows to manage user sessions.
    /// http://quickblox.com/developers/Authentication_and_Authorization
    /// </summary>
    public class AuthorizationClient
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;
        private readonly ICryptographicProvider cryptographicProvider;

        #endregion

        #region Ctor

        internal AuthorizationClient(QuickbloxClient client, ICryptographicProvider cryptographicProvider)
        {
            this.quickbloxClient = client;
            this.cryptographicProvider = cryptographicProvider;
        }

        #endregion

        /// <summary>
        /// Creates an Application session (not User seesion).
        /// </summary>
        /// <param name="deviceRequestRequest">Device info</param>
        /// <returns></returns>
        public async Task<HttpResponse<SessionResponse>> CreateSessionBaseAsync(DeviceRequest deviceRequestRequest = null)
        {
            var settings = this.CreateSessionRequest(quickbloxClient.ApplicationId, quickbloxClient.AuthKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.Signature = this.BuildSignatureFromJsonAttribute(quickbloxClient.AuthSecret, settings);
            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());

            return resultSessionResponse;
        }

        /// <summary>
        /// Creates a User session.
        /// </summary>
        /// <param name="userLogin">User login</param>
        /// <param name="userPassword">User password</param>
        /// <param name="provider">Provider</param>
        /// <param name="scope">Scope</param>
        /// <param name="socialToken">Social Token</param>
        /// <param name="socialSecret">Social secret</param>
        /// <param name="deviceRequestRequest">deviceInfo</param>
        /// <returns></returns>
        public async Task<HttpResponse<SessionResponse>> CreateSessionWithLoginAsync(String userLogin, String userPassword, String provider = null, String scope = null, String socialToken = null, String socialSecret = null, DeviceRequest deviceRequestRequest = null)
        {
            var settings = this.CreateSessionRequest(quickbloxClient.ApplicationId, quickbloxClient.AuthKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.User = new User() { Login = userLogin, Password = userPassword };
            settings.Scope = scope;

            if (!string.IsNullOrEmpty(socialToken) && !string.IsNullOrEmpty(socialSecret))
            {
                settings.SocialNetworkKey = new SocialNetworkKey() { Secret = socialSecret, Token = socialToken };
            }

            settings.Signature = this.BuildSignatureFromJsonAttribute(quickbloxClient.AuthSecret, settings);

            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());

            return resultSessionResponse;
        }

        /// <summary>
        /// Creates a User session.
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="userPassword">User password</param>
        /// <param name="provider">Provider</param>
        /// <param name="scope">Scope</param>
        /// <param name="socialToken">Social Token</param>
        /// <param name="socialSecret">Social secret</param>
        /// <param name="deviceRequestRequest">deviceInfo</param>
        /// <returns></returns>
        public async Task<HttpResponse<SessionResponse>> CreateSessionWithEmailAsync(String userEmail, String userPassword, String provider = null, String scope = null, String socialToken = null, String socialSecret = null, DeviceRequest deviceRequestRequest = null)
        {
            var settings = this.CreateSessionRequest(quickbloxClient.ApplicationId, quickbloxClient.AuthKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.User = new User() { Email = userEmail, Password = userPassword };
            settings.Scope = scope;
            settings.Provider = provider;

            if (!string.IsNullOrEmpty(socialToken))
            {
                settings.SocialNetworkKey = new SocialNetworkKey() { Secret = socialSecret, Token = socialToken };
            }

            settings.Signature = this.BuildSignatureFromJsonAttribute(quickbloxClient.AuthSecret, settings);

            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());
            return resultSessionResponse;
        }

        /// <summary>
        /// Creates a User session.
        /// </summary>
        /// <param name="provider">Provider</param>
        /// <param name="scope">Scope</param>
        /// <param name="socialToken">Social Token</param>
        /// <param name="socialSecret">Social secret</param>
        /// <param name="deviceRequestRequest">deviceInfo</param>
        /// <returns></returns>
        public async Task<HttpResponse<SessionResponse>> CreateSessionWithSocialNetworkKey(String provider = null, String scope = null, String socialToken = null, String socialSecret = null, DeviceRequest deviceRequestRequest = null)
        {
            var settings = this.CreateSessionRequest(quickbloxClient.ApplicationId, quickbloxClient.AuthKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.Scope = scope;
            settings.Provider = provider;

            if (!string.IsNullOrEmpty(socialToken))
            {
                settings.SocialNetworkKey = new SocialNetworkKey() { Secret = socialSecret, Token = socialToken };
            }

            settings.Signature = this.BuildSignatureFromJsonAttribute(quickbloxClient.AuthSecret, settings);

            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());
            return resultSessionResponse;
        }

        /// <summary>
        /// Deletes a session.
        /// </summary>
        /// <param name="token">Session token.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteSessionAsync(String token)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(token);
            var result = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, 
                                                                QuickbloxMethods.SessionMethod,
                                                                headers);
            return result;
        }

        /// <summary>
        /// Returns current session.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponse<SessionResponse>> GetSessionAsync()
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var result = await HttpService.GetAsync<SessionResponse>(this.quickbloxClient.ApiEndPoint,
                                                                QuickbloxMethods.SessionMethod,
                                                                headers);
            return result;
        }

        /// <summary>
        /// Upgrades current Application session to User session.
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="provider">Privider</param>
        /// <param name="scope">Scope</param>
        /// <returns></returns>
        public async Task<HttpResponse<LoginResponse>> ByLoginAsync(String login, String password, String provider = null, String scope= null)
        {
            
            var request = new LoginRequest();
            request.Login = login;
            request.Password = password;
            request.Provider = provider;
            request.Scope = scope;
            
            var loginResponse = await HttpService.PostAsync<LoginResponse, LoginRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                        QuickbloxMethods.LoginMethod,
                                                                                        request,
                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(quickbloxClient.Token));
            return loginResponse;
        }

        /// <summary>
        /// Upgrades current Application session to User session.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="provider">Privider</param>
        /// <param name="scope">Scope</param>
        /// <returns></returns>
        public async Task<HttpResponse<LoginResponse>> ByEmailAsync(String email, String password, String provider = null, String scope = null)
        {
            
            var request = new LoginRequest();
            request.Email = email;
            request.Password = password;
            request.Provider = provider;
            request.Scope = scope;

            var loginResponse = await HttpService.PostAsync<LoginResponse, LoginRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                        QuickbloxMethods.LoginMethod,
                                                                                        request,
                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(quickbloxClient.Token));
            return loginResponse;
        }

        #region Private methods

        private SessionRequest CreateSessionRequest(int applicationId, String authKey)
        {
            var settings = new SessionRequest
            {
                ApplicationId = applicationId,
                AuthKey = authKey,
                Timestamp = (long) DateTimeBuilder.ToUnixEpoch(DateTime.UtcNow)/1000,
                Nonce = new Random().Next(10000)
            };
            return settings;
        }

        private String BuildSignatureFromJsonAttribute(string authSecret, SessionRequest settings)
        {
            var properties = settings.GetType().GetRuntimeProperties();
            var navBody = new StringBuilder();
            var flag = false;
            foreach (
                var property in properties.Where(pr => pr.GetCustomAttribute<JsonPropertyAttribute>() != null).OrderBy(pr => pr.GetCustomAttribute<JsonPropertyAttribute>().PropertyName))
            {
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();

                if (property.PropertyType.GetTypeInfo().Namespace.Contains("Quickblox.Sdk"))
                {
                    var innerClass = property.GetValue(settings);
                    if (innerClass == null) continue;

                    var innerProperties = innerClass.GetType().GetRuntimeProperties();

                    foreach (var innerProperty in innerProperties.Where(pr => pr.GetCustomAttribute<JsonPropertyAttribute>() != null).OrderBy(pr => pr.GetCustomAttribute<JsonPropertyAttribute>().PropertyName))
                    {
                        var innerAttribute = innerProperty.GetCustomAttribute<JsonPropertyAttribute>();
                        var value = innerProperty.GetValue(innerClass);
                        if (value == null) continue;

                        if (flag)
                        {
                            navBody.Append(String.Format("&{0}[{1}]={2}", attribute.PropertyName, innerAttribute.PropertyName, value));
                            continue;
                        }
                        navBody.Append(String.Format("{0}[{1}]={2}", attribute.PropertyName, innerAttribute.PropertyName, value));
                        flag = true;
                    }
                }
                else
                {
                    var value = property.GetValue(settings);

                    if (value == null) continue;

                    if (flag)
                    {
                        navBody.Append(String.Format("&{0}={1}", attribute.PropertyName, value));
                        continue;
                    }
                    navBody.Append(String.Format("{0}={1}", attribute.PropertyName, value));
                    flag = true;
                }
            }

            return cryptographicProvider.Encrypt(navBody.ToString(), authSecret);
        }

        #endregion

    }
}
