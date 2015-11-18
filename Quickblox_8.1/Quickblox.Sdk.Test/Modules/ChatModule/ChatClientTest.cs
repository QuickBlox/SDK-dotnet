using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatModule.Responses;
using QMunicate.Logger;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace Quickblox.Sdk.Test.Modules.ChatModule
{
    [TestClass]
    public class ChatClientTest
    {
        private QuickbloxClient client;
        
        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new FileLogger());
            var sessionResponse = await this.client.AuthenticationClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "to1@test.com", "12345678");
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
            //aggregator.Filters.Add(new DialogSortFilter<int>(SortOperator.Asc, () => new DialogResponse().Type));
            aggregator.Filters.Add(new FieldFilter<int[]>(() => new DialogResponse().OccupantsIds, new int[] { 11879, 12779 }));

            retriveDialogsRequest.Filter = aggregator;
            var response = await this.client.ChatClient.GetDialogsAsync(retriveDialogsRequest);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetDialogsWithOperationFiltersSuccess()
        {
            var retriveDialogsRequest = new RetrieveDialogsRequest();

            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new SortFilter<int>(SortOperator.Asc, () => new DialogResponse().Type));
            aggregator.Filters.Add(new FieldFilterWithOperator<String>(SearchOperators.Lte, () => new DialogResponse().Id, "551d50bd535c123fc50260db"));

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
        public async Task GetMessagesWithFiltersTest()
        {
            var responseDialogs = await this.client.ChatClient.GetDialogsAsync();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);
            var testDialog = responseDialogs.Result.Items.First();

            var retrieveMessagesRequest = new RetrieveMessagesRequest();
            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new FieldFilter<string>(() => new Message().ChatDialogId, testDialog.Id));
            retrieveMessagesRequest.Filter = aggregator;

            var responseMessages = await this.client.ChatClient.GetMessagesAsync((RetrieveMessagesRequest)null);
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
