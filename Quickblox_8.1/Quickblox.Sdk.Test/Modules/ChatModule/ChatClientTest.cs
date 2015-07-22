using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatModule.Responses;

namespace Quickblox.Sdk.Test.Modules.ChatModule
{
    [TestClass]
    public class ChatClientTest
    {
        private QuickbloxClient client;
        
        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            var sessionResponse = await this.client.CoreClient.CreateSessionWithLoginAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");
            client.Token = sessionResponse.Result.Session.Token;
        }

        [TestMethod]
        public async Task CreateDialogSuccessTest()
        {
            var response = await this.client.ChatClient.CreateDialogAsync("New test dialog", DialogType.PublicGroup);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateGroupDialogTest()
        {
            string occupantsIds = "3323859,3323883";
            var response = await this.client.ChatClient.CreateDialogAsync("Weekend plans", DialogType.Group, occupantsIds, "http://monevator.monevator.netdna-cdn.com/wp-content/uploads/2008/12/small-cap-fireworks.jpg");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateMessageSuccessTest()
        {
            var responseCreateDialog = await this.client.ChatClient.CreateDialogAsync("New test dialog", DialogType.PublicGroup);
            Assert.AreEqual(responseCreateDialog.StatusCode, HttpStatusCode.Created);

            var createMessageRequest = new CreateMessageRequest() {ChatDialogId = responseCreateDialog.Result.Id, Message = "Hello"};
            var responseCreateMessage = await this.client.ChatClient.CreateMessageAsync(createMessageRequest);
            Assert.AreEqual(responseCreateMessage.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetDialogsSuccessTest()
        {
            var response = await this.client.ChatClient.GetDialogsAsync();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetDialogsWithFiltersSuccess()
        {
            var retriveDialogsRequest = new RetrieveDialogsRequest();

            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new DialogSortFilter<int>(SortOperator.Asc, () => new DialogResponse().Type));
            aggregator.Filters.Add(new RetrieveDialogsFilter<String>(() => new DialogResponse().Id, "551d50bd535c123fc50260db"));

            retriveDialogsRequest.Filter = aggregator;
            var response = await this.client.ChatClient.GetDialogsAsync(retriveDialogsRequest);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetDialogsWithOperationFiltersSuccess()
        {
            var retriveDialogsRequest = new RetrieveDialogsRequest();

            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new DialogSortFilter<int>(SortOperator.Asc, () => new DialogResponse().Type));
            aggregator.Filters.Add(new RetrieveDialogsFilterWithOperator<String>(DialogSearchOperator.Lte, () => new DialogResponse().Id, "551d50bd535c123fc50260db"));

            retriveDialogsRequest.Filter = aggregator;
            var response = await this.client.ChatClient.GetDialogsAsync(retriveDialogsRequest);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetMessagesSuccessTest()
        {
            var responseDialogs = await this.client.ChatClient.GetDialogsAsync();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var responseMessages = await this.client.ChatClient.GetMessagesAsync(responseDialogs.Result.Items.First().Id);
            Assert.AreEqual(responseMessages.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateDialogsSuccessTest()
        {
            var responseDialogs = await this.client.ChatClient.GetDialogsAsync();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var updateDialogRequest = new UpdateDialogRequest()
            {
                DialogId = responseDialogs.Result.Items.First().Id,
                Name = "New name 2"
            };
            var responseDelete = await this.client.ChatClient.UpdateDialogAsync(updateDialogRequest);
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseDelete.Result.Name, updateDialogRequest.Name);
        }

        [TestMethod]
        public async Task DeleteDialogsSuccessTest()
        {
            var responseDialogs = await this.client.ChatClient.GetDialogsAsync();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var responseDelete = await this.client.ChatClient.DeleteDialogAsync(responseDialogs.Result.Items.First().Id);
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
        }
    }
}
