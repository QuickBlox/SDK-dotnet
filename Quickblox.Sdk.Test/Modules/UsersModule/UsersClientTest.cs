using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Filter;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.UsersModule.Models;
using Quickblox.Sdk.Modules.UsersModule.Requests;

namespace Quickblox.Sdk.Test.Modules.UsersModule
{
    [TestClass]
    public class UsersClientTest
    {
        private const int ApplicationId = 21183;
        private const string AuthorizationKey = "LxnQksQJsXA2NLU";
        private const string AuthorizationSecret = "7v2Jkrc7e-99JJX";
        private const string Login = "Test654321";
        private const string Password = "Test12345";

        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient();
            await this.client.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await this.client.CoreClient.CreateSessionBaseAsync(ApplicationId, AuthorizationKey, AuthorizationSecret);
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
        public async Task ResetUserPasswordByEmailSuccess()
        {
            var response = await this.client.UsersClient.ResetUserPasswordByEmailAsync("test2@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        [TestMethod]
        public async Task RetrieveUsersUnauthorizedTest()
        {
            QuickbloxClient client2 = new QuickbloxClient();
            await client2.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            var response = await client2.UsersClient.RetrieveUsersAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
