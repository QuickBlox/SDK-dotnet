using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
            await client.CoreClient.CreateSessionWithLogin(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);
        }

        [TestMethod]
        public async Task RetrieveUsersUnauthorizedTest()
        {
            Quickblox.Sdk.QuickbloxClient client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey);
            await client2.InitializeClient();
            var  response = await client2.UsersClient.RetrieveUsers();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public async Task RetrieveUsersTest()
        {
            var response = await client.UsersClient.RetrieveUsers();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task SignUpUserTest()
        {

            var response = await client.UsersClient.SignUpUser("Test654321", "Test12345");

            Assert.IsTrue((int)response.StatusCode == 422); // login is aready taken
        }

        [TestMethod]
        public async Task GetUserTest()
        {
            var response = await client.UsersClient.GetUser(2766517);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByLoginTest()
        {

            var response = await client.UsersClient.GetUserByLogin("Test654321");

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFullNameTest()
        {
            var response = await client.UsersClient.GetUserByFullName("Test", null, null);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByFacebookTest()
        {
            var response = await client.UsersClient.GetUserByFacebookId(145);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetUserByEmail()
        {
            var response = await client.UsersClient.GetUserByEmail("test@test.com");

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task GetUserByTwitterTest()
        {
            var response = await client.UsersClient.GetUserByTwitterId(158564);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateUserTest()
        {
            var response = await client.UsersClient.UpdateUser("Test654321", 0, "test@test.com", 123, 145, 158, "Test", "0504654654", "SomeSite", null, null, null, null);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ResetUserPasswordByEmailTest()
        {
            var response = await client.UsersClient.ResetUserPasswordByEmail("test@test.com");
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

    }
}
