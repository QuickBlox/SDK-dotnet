using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.CustomObjectModule.Requests;

namespace Quickblox.Sdk.Test.Modules.CustomObjectModule
{
    [TestClass]
    public class CustomObjectClientTest
    {
        private const int ApplicationId = 21183;
        private const string AuthorizationKey = "LxnQksQJsXA2NLU";
        private const string AuthorizationSecret = "7v2Jkrc7e-99JJX";
        private const string Login = "Test654321";
        private const string Password = "Test12345";

        private const string ClassName = "TestCustomObject";

        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient();
            await this.client.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await this.client.CoreClient.CreateSessionWithLoginAsync(ApplicationId, AuthorizationKey, AuthorizationSecret, Login, Password);
        }

        [TestMethod]
        public async Task GetCustomObjects()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetCustomObjectsByIds()
        {
            var ids = "55630f456390d803e1018672";
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsByIdsAsync<TestCustomObjectModel>(ClassName, ids);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreateCustomObject()
        {
            var testCustomObjectModel = new CreateCustomObjectRequest<TestCustomObjectModel>();
            testCustomObjectModel.CreateCustomObject = new TestCustomObjectModel();
            testCustomObjectModel.CreateCustomObject.IntegerField = 111;
            testCustomObjectModel.CreateCustomObject.FloatField = 111.111;
            testCustomObjectModel.CreateCustomObject.BooleanField = true;
            testCustomObjectModel.CreateCustomObject.StringField = "Test1";
            testCustomObjectModel.CreateCustomObject.ArrayField = new List<int>() {123, 123, 123};
            var response = await this.client.CustomObjectsClient.CreateCustomObjectsAsync(ClassName, testCustomObjectModel);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
