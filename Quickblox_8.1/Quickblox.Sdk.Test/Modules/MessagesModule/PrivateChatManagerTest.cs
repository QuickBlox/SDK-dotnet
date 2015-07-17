using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class PrivateChatManagerTest
    {
        private static string email1 = "to1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 3323859;
        private static string jid1 = "3323859-21183@chat.quickblox.com";

        private static string email2 = "to2@test.com";
        private static string password2 = "12345678";
        private static int id2 = 3323883;
        private static string jid2 = "3323883-21183@chat.quickblox.com";

        private static QuickbloxClient client1;
        private static IPrivateChatManager chatManager1;
        private static List<Presence> lastPresencesClient1;

        private static QuickbloxClient client2;
        private static IPrivateChatManager chatManager2;
        private static Message lastMessageClient2;
        private static List<Presence> lastPresencesClient2;

        private static string dialogId = "55a3a744535c1219ce001064";

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            client1 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            //await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
#if DEBUG
            client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect("chat.quickblox.com", id1, (int)GlobalConstant.ApplicationId, password1);
            chatManager1 = client1.MessagesClient.GetPrivateChatManager(id2, dialogId);
            client1.MessagesClient.OnPresenceReceived += (sender, presence) => { if (lastPresencesClient1 != null) lastPresencesClient1.Add(presence); };

            client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            //await client2.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client2.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email2, password2);
#if DEBUG
            client2.MessagesClient.DebugClientName = "2";
#endif
            await client2.MessagesClient.Connect("chat.quickblox.com", id2, (int)GlobalConstant.ApplicationId, password2);
            chatManager2 = client2.MessagesClient.GetPrivateChatManager(id1, dialogId);
            client2.MessagesClient.OnMessageReceived += (sender, message) => lastMessageClient2 = message;
            client2.MessagesClient.OnPresenceReceived += (sender, presence) => { if(lastPresencesClient2 != null) lastPresencesClient2.Add(presence); };
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
        public async Task PresenceSubscribeTest()
        {
            lastPresencesClient2 = new List<Presence>();

            chatManager1.SubsribeForPresence();
            await Task.Delay(5000);

            if (!lastPresencesClient2.Any(p => p.From.Contains(jid1) && p.PresenceType == PresenceType.Subscribe))
                Assert.Fail("The presence subscribtion wasn't received by client2");
        }

        [TestMethod]
        public async Task PresenceApproveTest()
        {
            lastPresencesClient1 = new List<Presence>();

            chatManager2.ApproveSubscribtionRequest();
            await Task.Delay(5000);

            if (!lastPresencesClient1.Any(p => p.From.Contains(jid2) && p.PresenceType == PresenceType.Subscribed))
                Assert.Fail("The presence approval wasn't received by client1");
        }

        [TestMethod]
        public async Task DisconnectTest()
        {
            lastPresencesClient1 = new List<Presence>();

            client2.MessagesClient.Disconnect();
            await Task.Delay(5000);

            if (!lastPresencesClient1.Any(p => p.From.Contains(jid2) && p.PresenceType == PresenceType.Unavailable))
                Assert.Fail("Unavalibility wasn't received by client1");
        }

        /// <summary>
        /// Doesn't work. Not implemented yet for Ubiety libarary.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Doesn't work. Not implemented yet for Ubiety libarary.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PrivacyListTest()
        {
            string messageText = "Test message";

            await chatManager2.Unblock();

            lastMessageClient2 = null;
            chatManager1.SendMessage(messageText);
            await Task.Delay(5000);

            if (lastMessageClient2 == null || lastMessageClient2.MessageText != messageText)
                Assert.Fail("The message wasn't received by client2");

            await chatManager2.Block();

            lastMessageClient2 = null;
            chatManager1.SendMessage(messageText);
            await Task.Delay(5000);

            if (lastMessageClient2 != null)
                Assert.Fail("Blocking doesn't work");

        }
    }
}
