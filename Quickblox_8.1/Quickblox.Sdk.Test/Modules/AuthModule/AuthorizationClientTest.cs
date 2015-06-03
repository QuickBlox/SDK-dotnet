using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;

namespace Quickblox.Sdk.Test.Modules.AuthModule
{
    [TestClass]
    public class AuthorizationClientTest
    {
        private const uint ApplicationId = GlobalConstant.ApplicationId;
        private const string AuthorizationKey = GlobalConstant.AuthorizationKey;
        private const string AuthorizationSecret = GlobalConstant.AuthorizationSecret;
        private const string Login = "edward2";
        private const string Password = "edward123";
        private const string Email = "an@to.ly";

        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            client = new QuickbloxClient();
            await client.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
        }

        [TestMethod]
        public async Task CreateSessionBaseTest()
        {
            var response = await client.CoreClient.CreateSessionBaseAsync(ApplicationId, AuthorizationKey, AuthorizationSecret);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithLoginTest()
        {
            var response = await client.CoreClient.CreateSessionWithLoginAsync(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithEmailTest()
        {
            var response = await client.CoreClient.CreateSessionWithEmailAsync(ApplicationId, AuthorizationKey, AuthorizationSecret, Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetSessionTest()
        {
            var a = await client.CoreClient.CreateSessionWithLoginAsync(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            var response = await client.CoreClient.GetSessionAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSessionTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionWithLoginAsync(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);

            var response = await client.CoreClient.DeleteSessionAsync(sessionResponse.Result.Session.Token);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ByLoginTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionBaseAsync(ApplicationId, AuthorizationKey, AuthorizationSecret);

            var response = await client.CoreClient.ByLoginAsync(Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }

        [TestMethod]
        public async Task ByEmailTest()
        {
            var sessionResponse = await client.CoreClient.CreateSessionBaseAsync(ApplicationId, AuthorizationKey, AuthorizationSecret);

            var response = await client.CoreClient.ByEmailAsync(Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }
    }
}
