using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;

namespace Quickblox.Sdk.Test.Modules.ChatModule
{
    [TestClass]
    public class ChatClientTest
    {
        private QuickbloxClient client;

        public ChatClientTest()
        {
            client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey);
        }

        [TestMethod]
        public async Task CreateDialogSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var response = await this.client.ChatClient.CreateDialog("New test dialog", DialogType.PublicGroup);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateMessageSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var responseCreateDialog = await this.client.ChatClient.CreateDialog("New test dialog", DialogType.PublicGroup);
            Assert.AreEqual(responseCreateDialog.StatusCode, HttpStatusCode.Created);

            var createMessageRequest = new CreateMessageRequest() {ChatDialogId = responseCreateDialog.Result.Id, Message = "Hello"};
            var responseCreateMessage = await this.client.ChatClient.CreateMessage(createMessageRequest);
            Assert.AreEqual(responseCreateMessage.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetDialogsSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var response = await this.client.ChatClient.GetDialogs();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetMessagesSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var responseDialogs = await this.client.ChatClient.GetDialogs();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var responseMessages = await this.client.ChatClient.GetMessages(responseDialogs.Result.Items.First().Id);
            Assert.AreEqual(responseMessages.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateDialogsSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var responseDialogs = await this.client.ChatClient.GetDialogs();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var updateDialogRequest = new UpdateDialogRequest()
            {
                DialogId = responseDialogs.Result.Items.First().Id,
                Name = "New name 2"
            };
            var responseDelete = await this.client.ChatClient.UpdateDialog(updateDialogRequest);
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseDelete.Result.Name, updateDialogRequest.Name);
        }

        [TestMethod]
        public async Task DeleteDialogsSuccessTest()
        {
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");

            var responseDialogs = await this.client.ChatClient.GetDialogs();
            Assert.AreEqual(responseDialogs.StatusCode, HttpStatusCode.OK);

            var responseDelete = await this.client.ChatClient.DeleteDialog(responseDialogs.Result.Items.First().Id);
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
        }
    }
}
