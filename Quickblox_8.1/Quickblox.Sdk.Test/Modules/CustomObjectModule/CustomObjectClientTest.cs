using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.CustomObjectModule.Requests;
using Quickblox.Sdk.Modules.CustomObjectModule.Responses;

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
        private const string RelativeClassName = "TestRelativeObject";

        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            var sessionResponse = await this.client.CoreClient.CreateSessionWithLoginAsync(ApplicationId, AuthorizationKey, AuthorizationSecret,
                    Login, Password);

            client.Token = sessionResponse.Result.Session.Token;
        }

        [TestMethod]
        public async Task GetCustomObjects()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task GetCustomObjectsByIds()
        {
            var ids = "55630f456390d803e1018672";
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsByIdsAsync<TestCustomObjectModel>(ClassName, ids);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Result != null);
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
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task CreateMultiCustomObject()
        {
            var items = new List<TestCustomObjectModel>();
            var testCustomObjectModel = new TestCustomObjectModel();
            testCustomObjectModel.IntegerField = 111;
            testCustomObjectModel.FloatField = 111.111;
            testCustomObjectModel.BooleanField = true;
            testCustomObjectModel.StringField = "Multi1";
            testCustomObjectModel.ArrayField = new List<int>() { 123, 123, 123 };
            items.Add(testCustomObjectModel);

            testCustomObjectModel = new TestCustomObjectModel();
            testCustomObjectModel.IntegerField = 222;
            testCustomObjectModel.FloatField = 222.222;
            testCustomObjectModel.BooleanField = true;
            testCustomObjectModel.StringField = "Multi2";
            testCustomObjectModel.ArrayField = new List<int>() { 321, 321, 321 };
            items.Add(testCustomObjectModel);

            testCustomObjectModel = new TestCustomObjectModel();
            testCustomObjectModel.IntegerField = 333;
            testCustomObjectModel.FloatField = 333.333;
            testCustomObjectModel.BooleanField = true;
            testCustomObjectModel.StringField = "Multi3";
            testCustomObjectModel.ArrayField = new List<int>() { 333, 222, 111 };
            items.Add(testCustomObjectModel);

            var response = await this.client.CustomObjectsClient.CreateMultiCustomObjectsAsync(ClassName, items);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task UpdateCustomObject()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var updateCustomObjectRequest = new UpdateCustomObjectRequest<TestCustomObjectModel>();
            updateCustomObjectRequest.CustomObject = response.Result.Items.First();
            updateCustomObjectRequest.CustomObject.StringField = "Updated";

            var updateResponse = await this.client.CustomObjectsClient.UpdateCustomObjectsByIdAsync(ClassName, updateCustomObjectRequest);
            Assert.IsTrue(updateResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task UpdateMultiCustomObject()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            
            var list = new List<TestCustomObjectModel>();
            var first = response.Result.Items.First();
            var last = response.Result.Items.Last();
            first.StringField = "Updated - first";
            last.StringField = "Updated - last";

            list.Add(first);
            list.Add(last);
            var updateResponse = await this.client.CustomObjectsClient.UpdateMultiCustomObjectsAsync(ClassName, list);
            Assert.IsTrue(updateResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task CreateRelationObject()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var parentItem = response.Result.Items.First();
            var createCustomObjectRequest = new CreateCustomObjectRequest<TestRelativeObject>();
            createCustomObjectRequest.CreateCustomObject = new TestRelativeObject()
            {
                RelativeString = "This item relative to " + parentItem.Id
            };

            var updateResponse = await this.client.CustomObjectsClient.CreateRelationObjectAsync(ClassName, parentItem.Id, RelativeClassName,  createCustomObjectRequest);
            Assert.IsTrue(updateResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(response.Result != null);
        }

        [TestMethod]
        public async Task GetRelationObjects()
        {
            var response = await this.client.CustomObjectsClient.RetriveCustomObjectsAsync<TestCustomObjectModel>(ClassName);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var parentItem = response.Result.Items.First();
            
            var retriveResponse = await this.client.CustomObjectsClient.RetriveRelationObjectsAsync<TestRelativeObject>(ClassName, parentItem.Id, RelativeClassName);
            Assert.IsTrue(retriveResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Result != null);
        }
    }
}
