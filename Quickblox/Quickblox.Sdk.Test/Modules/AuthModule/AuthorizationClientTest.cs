using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Platform;

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
            client = new QuickbloxClient((int)ApplicationId, AuthorizationKey, AuthorizationSecret, GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
        }

        [TestMethod]
        public async Task CreateSessionBaseTest()
        {
            var response = await client.AuthenticationClient.CreateSessionBaseAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithLoginTest()
        {
            var response = await client.AuthenticationClient.CreateSessionWithLoginAsync(Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateSessionWithEmailTest()
        {
            var response = await client.AuthenticationClient.CreateSessionWithEmailAsync(Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetSessionTest()
        {
            var a = await client.AuthenticationClient.CreateSessionWithLoginAsync(Login, Password);
            var response = await client.AuthenticationClient.GetSessionAsync();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSessionTest()
        {
            var sessionResponse = await client.AuthenticationClient.CreateSessionWithLoginAsync(Login, Password);
            var response = await client.AuthenticationClient.DeleteSessionAsync(sessionResponse.Result.Session.Token);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ByLoginTest()
        {
            var sessionResponse = await client.AuthenticationClient.CreateSessionBaseAsync();

            var response = await client.AuthenticationClient.ByLoginAsync(Login, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }

        [TestMethod]
        public async Task ByEmailTest()
        {
            var sessionResponse = await client.AuthenticationClient.CreateSessionBaseAsync();

            var response = await client.AuthenticationClient.ByEmailAsync(Email, Password);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Accepted);
        }
    }
}
