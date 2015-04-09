using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Modules.UsersModule.Requests;

namespace Quickblox.Sdk.Test.Modules.UsersModule
{
    [TestClass]
    public class UsersClientTest
    {
        private const string ApplicationId = "21183";
        private const string AuthorizationKey = "LxnQksQJsXA2NLU";
        private const string AuthorizationSecret = "7v2Jkrc7e-99JJX";
        private const string Login = "Test654321";
        private const string Password = "Test12345";

        private Quickblox.Sdk.QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey);
            await client.InitializeClient();
            await client.CoreClient.CreateSessionBase(ApplicationId, AuthorizationKey, AuthorizationSecret);
        }
        
        [TestMethod]
        public async Task GetUserByLoginSuccess()
        {
            await client.CoreClient.ByLogin(Login, Password);
            var response = await client.UsersClient.GetUserByLogin("Test654321");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByEmailSuccess()
        {
            var response = await client.UsersClient.GetUserByEmail("dark_angel2891@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFacebookIdSuccess()
        {
            var response = await client.UsersClient.GetUserByFacebookId(100007825392773);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByTwitterIdSuccess()
        {
            var response = await client.UsersClient.GetUserByTwitterId(158564);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task RetrieveUsersSuccess()
        {
            var response = await client.UsersClient.RetrieveUsers();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task SignUpUserSuccess()
        {
            var response = await client.UsersClient.SignUpUser("Test1234567", "qwerty123456", email:"test2@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created); // login is aready taken
        }

        [TestMethod]
        public async Task GetUserByIdSuccess()
        {
            var response = await client.UsersClient.GetUserById(2766517);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFullNameSuccess()
        {
            var response = await client.UsersClient.GetUserByFullName("Eduardo", null, null);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        [TestMethod]
        public async Task UpdateUserSuccess()
        {
            var loginResponse = await client.CoreClient.ByLogin("Test654321", Password);
            Assert.IsTrue(loginResponse.StatusCode == HttpStatusCode.Accepted);

            var updateUserRequest = new UpdateUserRequest();
            updateUserRequest.User = new UserRequest()
            {
                Email = "test3@mail.ru",
                FullName = "Eduardo2",
            };

            var responseUpdateUser = await client.UsersClient.UpdateUser(loginResponse.Result.User.Id, updateUserRequest);

            Assert.IsTrue(responseUpdateUser.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(responseUpdateUser.Result.User.FullName == updateUserRequest.User.FullName);
            Assert.IsTrue(responseUpdateUser.Result.User.Email == updateUserRequest.User.Email);
        }

        [TestMethod]
        public async Task ResetUserPasswordByEmailSuccess()
        {
            var response = await client.UsersClient.ResetUserPasswordByEmail("test2@mail.ru");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        [TestMethod]
        public async Task RetrieveUsersUnauthorizedTest()
        {
            Quickblox.Sdk.QuickbloxClient client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey);
            await client2.InitializeClient();
            var response = await client2.UsersClient.RetrieveUsers();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Unauthorized);
        }
    }
}
