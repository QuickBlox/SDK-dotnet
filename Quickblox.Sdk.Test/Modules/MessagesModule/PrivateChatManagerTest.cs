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
        private static string login1 = "Test654321";
        private static string password1 = "Test12345";
        private static int id1 = 2701456;

        private static string login2 = "Test987654";
        private static string password2 = "Test12345";
        private static int id2 = 2766517;


        private static QuickbloxClient client1;
        private static IPrivateChatManager chatManager1;

        private static QuickbloxClient client2;
        private static IPrivateChatManager chatManager2;
        private static Message lastMessageClient2;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            client1 = new QuickbloxClient();
            await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await client1.CoreClient.CreateSessionWithLoginAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, login1, password1);
            await client1.MessagesClient.Connect(id1, password1, (int)GlobalConstant.ApplicationId, "chat.quickblox.com", new TimeSpan(0, 0, 10));
            chatManager1 = client1.MessagesClient.GetPrivateChatManager(id2);

            client2 = new QuickbloxClient();
            await client2.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await client2.CoreClient.CreateSessionWithLoginAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, login2, password2);
            await client2.MessagesClient.Connect(id2, password2, (int)GlobalConstant.ApplicationId, "chat.quickblox.com", new TimeSpan(0, 0, 10));
            chatManager2 = client2.MessagesClient.GetPrivateChatManager(id1);
            client2.MessagesClient.OnMessageReceived += delegate(object sender, Message message)
            {
                lastMessageClient2 = message;
            };
        }

        [TestMethod]
        public async Task SendMessageTest()
        {
            lastMessageClient2 = null;
            string messageText = "Test message";

            chatManager1.SendMessage(messageText);
            await Task.Delay(5000);

            if (lastMessageClient2 == null || lastMessageClient2.MessageText != messageText)
                Assert.Fail("The message wasn't received by client2");
        }

        [TestMethod]
        public async Task SendMessageAttachmentTest()
        {
            lastMessageClient2 = null;
            string attachemntId = "543534";
            chatManager1.SendMessage("Test message", new Attachment() { Id = attachemntId, Type = "photo" });

            await Task.Delay(5000);

            if (lastMessageClient2 == null || lastMessageClient2.Attachments == null || lastMessageClient2.Attachments.Count() != 1 || lastMessageClient2.Attachments[0].Id != attachemntId)
                Assert.Fail("The message wasn't received correctly by client 2.");
        }
    }
}
