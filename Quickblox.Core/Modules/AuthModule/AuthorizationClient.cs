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
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk.Modules.AuthModule
{
    public class AuthorizationClient
    {
        private readonly QuickbloxClient quickbloxClient;

        public AuthorizationClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }
        
        public async Task<HttpResponse<SessionResponse>> CreateSessionBaseAsync(UInt32 applicationId, String authKey, String authSecret, DeviceRequest deviceRequestRequest = null)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = this.CreateSessionRequest(applicationId, authKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.Signature = this.BuildSignatureFromJsonAttribute(authSecret, settings);
            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());

            this.quickbloxClient.Token = resultSessionResponse.Result.Session.Token;
            return resultSessionResponse;
        }

        public async Task<HttpResponse<SessionResponse>> CreateSessionWithLoginAsync(UInt32 applicationId, String authKey, String authSecret, String userLogin, String userPassword, String provider = null, SocialScope? scope = null, String socialToken = null, String socialSecret = null, DeviceRequest deviceRequestRequest = null)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = this.CreateSessionRequest(applicationId, authKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.User = new User() { Login = userLogin, Password = userPassword };
            settings.Scope = scope;

            if (!string.IsNullOrEmpty(socialToken) && !string.IsNullOrEmpty(socialSecret))
            {
                settings.SocialNetworkKey = new SocialNetworkKey() { Secret = socialSecret, Token = socialToken };
            }

            settings.Signature = this.BuildSignatureFromJsonAttribute(authSecret, settings);

            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());

            this.quickbloxClient.Token = resultSessionResponse.Result.Session.Token;
            return resultSessionResponse;
        }

        public async Task<HttpResponse<SessionResponse>> CreateSessionWithEmailAsync(UInt32 applicationId, String authKey, String authSecret, String userLogin, String userPassword, String provider = null, SocialScope? scope = null, String socialToken = null, String socialSecret = null, DeviceRequest deviceRequestRequest = null)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = this.CreateSessionRequest(applicationId, authKey);
            settings.DeviceRequest = deviceRequestRequest;
            settings.User = new User() { Email = userLogin, Password = userPassword };
            settings.Scope = scope;

            if (!string.IsNullOrEmpty(socialToken) && !string.IsNullOrEmpty(socialSecret))
            {
                settings.SocialNetworkKey = new SocialNetworkKey() { Secret = socialSecret, Token = socialToken };
            }

            settings.Signature = this.BuildSignatureFromJsonAttribute(authSecret, settings);

            var resultSessionResponse = await HttpService.PostAsync<SessionResponse, SessionRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders());
            if(resultSessionResponse.Result != null && resultSessionResponse.Result.Session != null)
            this.quickbloxClient.Token = resultSessionResponse.Result.Session.Token;
            return resultSessionResponse;
        }

        public async Task<HttpResponse> DeleteSessionAsync(String token)
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var result = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, 
                                                                QuickbloxMethods.SessionMethod,
                                                                headers);
            return result;
        }

        public async Task<HttpResponse<SessionResponse>> GetSessionAsync()
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var result = await HttpService.GetAsync<SessionResponse>(this.quickbloxClient.ApiEndPoint,
                                                                QuickbloxMethods.SessionMethod,
                                                                headers);
            return result;
        }

        public async Task<HttpResponse<LoginResponse>> ByLoginAsync(String login, String password, String provider = null, String scope= null)
        {
            this.quickbloxClient.CheckIsInitialized();
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

        public async Task<HttpResponse<LoginResponse>> ByEmailAsync(String email, String password, String provider = null, String scope = null)
        {
            this.quickbloxClient.CheckIsInitialized();
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

        private SessionRequest CreateSessionRequest(UInt32 applicationId, String authKey)
        {
            var settings = new SessionRequest();
            settings.ApplicationId = applicationId;
            settings.AuthKey = authKey;
            settings.Timestamp = (long)DateTimeBuilder.ToUnixEpoch(DateTime.Now) / 1000;
            settings.Nonce = new Random().Next(10000);
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

            return quickbloxClient.CryptographicProvider.Encrypt(navBody.ToString(), authSecret);
        }
    }
}
