using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class PrivateChatManagerTest
    {
        private QuickbloxClient client;
        private IPrivateChatManager chatManager;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await this.client.InitializeClientAsync();
            await this.client.CoreClient.CreateSessionWithLoginAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345");
            await client.MessagesClient.Connect(2701456, "Test12345", (int)GlobalConstant.ApplicationId, "chat.quickblox.com", new TimeSpan(0, 0, 10));
            chatManager = client.MessagesClient.GetPrivateChatManager(2766517);
        }

        [TestMethod]
        public async Task SendMessageTest()
        {
            chatManager.SendMessage("Test message", new Attachment(){Id = "543534", Type = "photo"});
        }
    }
}
