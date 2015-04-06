using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.UsersModule.Requests;
using Quickblox.Sdk.Modules.UsersModule.Responses;

namespace Quickblox.Sdk.Modules.UsersModule
{
    public class UsersClient
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        public UsersClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods

        public async Task<HttpResponse<RetrieveUsersResponse>> RetrieveUsers()
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveUsersResponse>(this.quickbloxClient.ApiEndPoint, QuickbloxMethods.UsersMethod, new NewtonsoftJsonSerializer(), headers);
        }

        public async Task<HttpResponse<UserResponse>> SignUpUser(UserSignUpRequest userSignUpRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<UserResponse, UserSignUpRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.UsersMethod,
                        new NewtonsoftJsonSerializer(), userSignUpRequest, headers);
        }

        #endregion
    }
}
