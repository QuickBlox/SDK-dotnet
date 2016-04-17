using System;
using System.Threading;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.UsersModule.Requests;
using Quickblox.Sdk.Modules.UsersModule.Responses;
using Quickblox.Sdk.Serializer;
using Quickblox.Sdk.Modules.UsersModule.Models;

namespace Quickblox.Sdk.Modules.UsersModule
{
    /// <summary>
    /// Provide methods to User module API
    /// </summary>
    public class UsersClient
    {
        #region Fields

        private readonly IQuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient"/> class.
        /// </summary>
        /// <param name="quickbloxClient">The quickblox client.</param>
        internal UsersClient(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieve all Users for current account
        /// </summary>
        /// <param name="retrieveUsersesRequest">Filter settings</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveUsersResponse>> RetrieveUsersAsync(RetrieveUsersRequest retrieveUsersesRequest = null)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveUsersResponse, RetrieveUsersRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.UsersMethod, retrieveUsersesRequest, headers);
        }

        /// <summary>
        /// Retrieve all Users for current account. Return custom User's model that extended UserModule.Models.User class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retrieveUsersesRequest">Filter settings</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveUsersResponse<T>>> RetrieveUsersAsync<T>(RetrieveUsersRequest retrieveUsersesRequest = null) where T : User
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveUsersResponse<T>, RetrieveUsersRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.UsersMethod, retrieveUsersesRequest, headers);
        }

        /// <summary>
        /// API User sign up. Use for the identification of the mobile applications users. The request can contain all, some or none of the optional parameters.
        /// Login, email, facebook ID, twitter ID and the external user ID should not be taken previously.
        /// If you want to create a user with a some content (f.e. with a photo) you have to create a blob firstly.
        /// The same tags can be used for any number of users.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> SignUpUserAsync(UserSignUpRequest userSignUpRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<UserResponse, UserSignUpRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.UsersMethod, userSignUpRequest, headers);
        }

        /// <summary>
        /// Show API User by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByIdAsync(Int32 userId)
        {
            var uriBuilder = String.Format(QuickbloxMethods.GetUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse>(this.quickbloxClient.ApiEndPoint, uriBuilder, headers);
        }

        /// <summary>
        /// Show API User by identifier (generic)
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse<T>>> GetUserByIdAsync<T>(Int32 userId) where T : User
        {
            var uriBuilder = String.Format(QuickbloxMethods.GetUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse<T>>(this.quickbloxClient.ApiEndPoint, uriBuilder, headers);
        }

        /// <summary>
        /// Gets the user by login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByLogin(String login)
        {
            var byLogin = new UserRequest() {Login = login};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByLoginMethod, byLogin, headers);
        }

        /// <summary>
        /// Gets the full name of the user by.
        /// </summary>
        /// <param name="fullName">API User full name</param>
        /// <param name="page">Page number of the book of the results that you want to get. By default: 1</param>
        /// <param name="perPage">The maximum number of results per page. Min: 1. Max: 100. By default: 10</param>
        /// <returns></returns>
        public async Task<HttpResponse<PagedResponse<UserResponse>>> GetUserByFullNameAsync(String fullName, UInt32? page, UInt32? perPage, CancellationToken token = default(CancellationToken))
        {
            var byFullname = new UserRequest()
            {
                FullName = fullName,
                Page = page,
                PerPage = perPage
            };
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<PagedResponse<UserResponse>, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByFullMethod, byFullname, headers, token: token);
        }

        /// <summary>
        /// Search API User by Facebook identifier.
        /// </summary>
        /// <param name="facebookId">API User Facebook ID</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByFacebookIdAsync(Int64 facebookId)
        {
            var byFacebook = new UserRequest() { FacebookId = facebookId };
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByFacebookIdMethod, byFacebook, headers);
        }

        /// <summary>
        /// Retrieve API User by Twitter identifier
        /// </summary>
        /// <param name="twitterId">API User Twitter ID</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByTwitterIdAsync(Int32 twitterId)
        {
            var byTwitter = new UserRequest() { TwitterId = twitterId };
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByTwitterIdMethod, byTwitter, headers);
        }

        /// <summary>
        /// Retrieve API User by email.
        /// </summary>
        /// <param name="email">API User email</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByEmailAsync(String email)
        {
            var byEmail = new UserRequest() { Email = email };
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByEmailMethod, byEmail, headers);
        }

        /// <summary>
        /// Search API Users by tags
        /// </summary>
        /// <param name="tags">API User tag(s) The maximum number of tags per user: 5.</param>
        /// <param name="page">Page number of the book of the results that you want to get. By default: 1</param>
        /// <param name="perPage">The maximum number of results per page. Min: 1. Max: 100. By default: 10</param>
        /// <returns></returns>
        public async Task<HttpResponse<PagedResponse<UserResponse>>> GetUserByTagsAsync(String[] tags, UInt32 page = 1, UInt32 perPage = 10)
        {
            var byUserTag = new UserRequestWithTag()
            {
                Tags = String.Join(",", tags),
                Page = page,
                PerPage = perPage
            };

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<PagedResponse<UserResponse>, UserRequestWithTag>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByTagsMethod, byUserTag, headers);
        }

        /// <summary>
        /// Retrieve API User by external user id
        /// </summary>
        /// <param name="externalUserId">The external user Id.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByExeternalUserIdAsync(Int32 externalUserId)
        {
            var uriMethod = String.Format(QuickbloxMethods.GetUserByExternalUserMethod, externalUserId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse>(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
        }

        /// <summary>
        /// Update API User by identifier
        /// </summary>
        /// <param name="userRequest">Agregate all user parameters</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> UpdateUserAsync(Int32 userId, UpdateUserRequest userRequest)
        {
            var uriMethod = string.Format(QuickbloxMethods.UpdateUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<UserResponse, UpdateUserRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, userRequest, headers);
        }

        /// <summary>
        /// Updates the user by Id (generic).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userRequest">The user request.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> UpdateUserAsync<T>(Int32 userId, UpdateUserRequest<T> userRequest) where T : UserRequest
        {
            var uriMethod = string.Format(QuickbloxMethods.UpdateUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<UserResponse, UpdateUserRequest<T>>(this.quickbloxClient.ApiEndPoint, uriMethod, userRequest, headers);
        }

        /// <summary>
        /// Delete API User by id
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteUserByIdAsync(Int32 userId)
        {
            var uriMethod = String.Format(QuickbloxMethods.DeleteUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
        }

        /// <summary>
        /// Delete API User by external user id
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteUserByExternalUserIdAsync(Int32 userId)
        {
            var uriMethod = String.Format(QuickbloxMethods.DeleteUserByExternalUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
        }

        /// <summary>
        /// Reset API User password by e-mail.
        /// </summary>
        /// <param name="email">API User e-mail.</param>
        /// <returns></returns>
        public async Task<HttpResponse> ResetUserPasswordByEmailAsync(String email)
        {
            var userRequest = new UserRequest()
            {
                Email = email
            };

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<Object, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.UserPasswordResetMethod, userRequest, headers);
        }

        #endregion
    }
}
