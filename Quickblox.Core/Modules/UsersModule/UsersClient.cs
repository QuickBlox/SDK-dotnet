using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.UsersModule.Requests;
using Quickblox.Sdk.Modules.UsersModule.Responses;
using Quickblox.Sdk.Serializer;

namespace Quickblox.Sdk.Modules.UsersModule
{
    /// <summary>
    /// Provide methods to User module API
    /// </summary>
    public class UsersClient
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient"/> class.
        /// </summary>
        /// <param name="quickbloxClient">The quickblox client.</param>
        public UsersClient(QuickbloxClient quickbloxClient)
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
        public async Task<HttpResponse<RetrieveUsersResponse>> RetrieveUsers(RetrieveUsersRequest retrieveUsersesRequest = null)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveUsersResponse, RetrieveUsersRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.UsersMethod, retrieveUsersesRequest, headers);
        }

        /// <summary>
        /// API User sign up. Use for the identification of the mobile applications users. The request can contain all, some or none of the optional parameters.
        /// Login, email, facebook ID, twitter ID and the external user ID should not be taken previously.
        /// If you want to create a user with a some content (f.e. with a photo) you have to create a blob firstly.
        /// The same tags can be used for any number of users.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="blobId">ID of associated blob (for example, API User photo).</param>
        /// <param name="email">The email.</param>
        /// <param name="externalUserId">The external user identifier.</param>
        /// <param name="facebookId">The facebook identifier.</param>
        /// <param name="twitterId">The twitter identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="website">The website.</param>
        /// <param name="tagList">The tag list.</param>
        /// <param name="customData">The custom data.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> SignUpUser(String login, String password, Int32? blobId = null, String email = null, Int32? externalUserId = null, Int32? facebookId = null, Int32? twitterId = null, String fullName = null, String phone = null, String website = null, String[] tagList = null, String customData  = null)
        {
            var userSignUpRequest = new UserSignUpRequest();
            userSignUpRequest.User = new UserRequest()
            {
                Login = login,
                Email = email,
                FullName = fullName,
                Phone = phone,
                Website = website,
                TagList = tagList == null ? null : String.Join(",", tagList),
                CustomData = customData,
                Password = password
            };

            userSignUpRequest.User.ExternalUserId = externalUserId;
            userSignUpRequest.User.BlobId = blobId;
            userSignUpRequest.User.FacebookId = facebookId;
            userSignUpRequest.User.TwitterId = twitterId;

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<UserResponse, UserSignUpRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.UsersMethod, userSignUpRequest, headers);
        }

        /// <summary>
        /// Show API User by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserById(Int32 userId)
        {
            var uriBuilder = String.Format(QuickbloxMethods.GetUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<UserResponse>(this.quickbloxClient.ApiEndPoint, uriBuilder, headers);
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
        public async Task<HttpResponse<PagedResponse<UserResponse>>> GetUserByFullName(String fullName, UInt32? page, UInt32? perPage)
        {
            var byFullname = new UserRequest()
            {
                FullName = fullName,
                Page = page,
                PerPage = perPage
            };
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<PagedResponse<UserResponse>, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByFullMethod, byFullname, headers);
        }

        /// <summary>
        /// Search API User by Facebook identifier.
        /// </summary>
        /// <param name="facebookId">API User Facebook ID</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByFacebookId(Int64 facebookId)
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
        public async Task<HttpResponse<UserResponse>> GetUserByTwitterId(Int32 twitterId)
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
        public async Task<HttpResponse<UserResponse>> GetUserByEmail(String email)
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
        public async Task<HttpResponse<PagedResponse<UserResponse>>> GetUserByTags(String[] tags, UInt32? page, UInt32 perPage)
        {
            var byUserTag = new UserRequest()
            {
                TagList = String.Join(",", tags),
                Page = page,
                PerPage = perPage
            };

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<PagedResponse<UserResponse>, UserRequest>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.GetUserByTwitterIdMethod, byUserTag, headers);
        }

        /// <summary>
        /// Retrieve API User by external user id
        /// </summary>
        /// <param name="externalUserId">The external user Id.</param>
        /// <returns></returns>
        public async Task<HttpResponse<UserResponse>> GetUserByExeternalUserId(Int32 externalUserId)
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
        public async Task<HttpResponse<UserResponse>> UpdateUser(Int32 userId, UpdateUserRequest userRequest)
        {
            var uriMethod = string.Format(QuickbloxMethods.UpdateUserMethod, userId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<UserResponse, UpdateUserRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, userRequest, headers);
        }

        /// <summary>
        /// Delete API User by id
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteUserById(Int32 userId)
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
        public async Task<HttpResponse> DeleteUserByExternalUserId(Int32 userId)
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
        public async Task<HttpResponse> ResetUserPasswordByEmail(String email)
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
