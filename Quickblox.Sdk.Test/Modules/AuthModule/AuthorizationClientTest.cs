using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;

namespace Quickblox.Sdk.Test.Modules.AuthModule
{
    [TestClass]
    public class AuthorizationClientTest
    {
        private const int ApplicationId = 21183;
        private const string AuthorizationKey = "LxnQksQJsXA2NLU";
        private const string AuthorizationSecret = "7v2Jkrc7e-99JJX";
        private const string Login = "Test654321";
        private const string Password = "Test12345";
        private const string Email = "test@test.com";

        private Quickblox.Sdk.QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await client.InitializeClient();
        }

        [TestMethod]
        public async Task CreateSessionBaseTest()
        {
            var response = await client.CoreClient.CreateSessionBase(ApplicationId, AuthorizationKey, AuthorizationSecret);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithLoginTest()
        {
            var response = await client.CoreClient.CreateSessionWithLogin(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithEmailTest()
        {
            var response = await client.CoreClient.CreateSessionWithEmail(ApplicationId, AuthorizationKey, AuthorizationSecret, Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetSessionTest()
        {
            var a = await client.CoreClient.CreateSessionWithLogin(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            var response = await client.CoreClient.GetSession();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSessionTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionWithLogin(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            var response = await client.CoreClient.DeleteSession(sessionResponse.Result.Session.Token);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ByLoginTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionBase(ApplicationId, AuthorizationKey, AuthorizationSecret);

            var response = await client.CoreClient.ByLogin(Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }

        [TestMethod]
        public async Task ByEmailTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionBase(ApplicationId, AuthorizationKey, AuthorizationSecret);

            var response = await client.CoreClient.ByEmail(Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }
    }
}
