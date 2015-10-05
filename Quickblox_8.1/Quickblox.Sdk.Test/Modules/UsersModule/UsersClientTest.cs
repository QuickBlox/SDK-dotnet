using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Filter;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.UsersModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Requests;
using Quickblox.Sdk.Test.Modules.UsersModule.Models;

namespace Quickblox.Sdk.Test.Modules.UsersModule
{
    [TestClass]
    public class UsersClientTest
    {
        private const uint ApplicationId = GlobalConstant.ApplicationId;
        private const string AuthorizationKey = GlobalConstant.AuthorizationKey;
        private const string AuthorizationSecret = GlobalConstant.AuthorizationSecret;
        private const string Login = "Test654321";
        private const string Password = "Test12345";

        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint);
            var sessionResponse = await this.client.CoreClient.CreateSessionBaseAsync(ApplicationId, AuthorizationKey, AuthorizationSecret);
            client.Token = sessionResponse.Result.Session.Token;
        }
        
        [TestMethod]
        public async Task GetUserByLoginSuccess()
        {
            await this.client.CoreClient.ByLoginAsync(Login, Password);
            var response = await this.client.UsersClient.GetUserByLogin("Test654321");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByEmailSuccess()
        {
            var response = await this.client.UsersClient.GetUserByEmailAsync("dark_angel2891@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFacebookIdSuccess()
        {
            var response = await this.client.UsersClient.GetUserByFacebookIdAsync(100007825392773);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByTwitterIdSuccess()
        {
            var response = await this.client.UsersClient.GetUserByTwitterIdAsync(158564);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task RetrieveUsersSuccess()
        {
            var response = await this.client.UsersClient.RetrieveUsersAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task RetrieveUsersWithFilterSuccess()
        {
            var filterAgregator = new FilterAggregator();
            filterAgregator.Filters.Add(new RetrieveUserFilter<String>(UserOperator.Eq, () => new User().FullName, "Eduardo"));
            filterAgregator.Filters.Add(new UserSortFilter<String>(SortOperator.Asc, () => new User().FullName));
            var retriveUserRequest = new RetrieveUsersRequest()
            {
                Filter = filterAgregator
            };

            var response = await this.client.UsersClient.RetrieveUsersAsync(retriveUserRequest);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task SignUpUserSuccess()
        {
            var response = await this.client.UsersClient.SignUpUserAsync("Test1234567", "qwerty123456", email:"test2@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created); // login is aready taken
        }

        [TestMethod]
        public async Task GetUserByIdSuccess()
        {
            var response = await this.client.UsersClient.GetUserByIdAsync(2766517);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFullNameSuccess()
        {
            var response = await this.client.UsersClient.GetUserByFullNameAsync("Eduardo", null, null);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        [TestMethod]
        public async Task UpdateUserSuccess()
        {
            var loginResponse = await this.client.CoreClient.ByLoginAsync("Test654321", Password);
            Assert.IsTrue(loginResponse.StatusCode == HttpStatusCode.Accepted);

            var updateUserRequest = new UpdateUserRequest();
            updateUserRequest.User = new UserRequest()
            {
                Email = "test3@mail.ru",
                FullName = "Eduardo2",
            };

            var responseUpdateUser = await this.client.UsersClient.UpdateUserAsync(loginResponse.Result.User.Id, updateUserRequest);

            Assert.IsTrue(responseUpdateUser.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(responseUpdateUser.Result.User.FullName == updateUserRequest.User.FullName);
            Assert.IsTrue(responseUpdateUser.Result.User.Email == updateUserRequest.User.Email);
        }

        [TestMethod]
        public async Task UpdateUserGericSuccess()
        {
            var accessTokenFB = "CAAFYnUVKERcBAPPgCYPqm4UZB19SZBZAlkTMQMhZByMipETIJfeZAbjVYp6xf9usgAbxRsLEmvsuPHzgASr4HW62Bj71HKGgDBTdq4PamjQWpQgBbm9OVHoDoJPMluxLOZA73KVfMS5OeL529WCYJbdRTgAgNcZAlrQxRZBTcFknwJZC5bZCNiGhbbjTDE6DcZAbWcZD";
            var sessionResponse = await client.CoreClient.CreateSessionWithSocialNetworkKey(ApplicationId, AuthorizationKey, AuthorizationSecret, "facebook",
                                                                "public_profile",
                                                                accessTokenFB,
                                                                null,
                                                                null);
            this.client.Token = sessionResponse.Result.Session.Token;

            var extndedRequest = new UpdateUserRequest<ExtendedUserRequest>();
            extndedRequest.User = new ExtendedUserRequest();
            extndedRequest.User.ShowMe = ShowMe.all;
            extndedRequest.User.Gender = Gender.female;
            extndedRequest.User.AboutMe = "asdasdad";
            extndedRequest.User.Email = "dasdad@mail.ru";
            var responseUpdateUser = await this.client.UsersClient.UpdateUserAsync(sessionResponse.Result.Session.UserId, extndedRequest);

            Assert.IsTrue(responseUpdateUser.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task ResetUserPasswordByEmailSuccess()
        {
            var response = await this.client.UsersClient.ResetUserPasswordByEmailAsync("test2@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        [TestMethod]
        public async Task RetrieveUsersUnauthorizedTest()
        {
            QuickbloxClient client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint);
            var response = await client2.UsersClient.RetrieveUsersAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
