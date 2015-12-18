using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Interfaces;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Quickblox.Sdk.Test.Logger;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class PrivateChatManagerTest
    {
        public const string ApiBaseEndPoint = "https://api.quickblox.com";
        public const string ChatEndpoint = "chat.quickblox.com";

        public const int ApplicationId = 13318;
        public const string AuthorizationKey = "WzrAY7vrGmbgFfP";
        public const string AuthorizationSecret = "xS2uerEveGHmEun";

        private static string email1 = "user1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 5719149;
        private static string jid1 = $"{id1}-{ApplicationId}@chat.quickblox.com";

        private static string email2 = "user2@test.com";
        private static string password2 = "12345678";
        private static int id2 = 5513419;
        private static string jid2 = $"{id2}-{ApplicationId}@chat.quickblox.com";

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
            var logger = new DebugLogger();

            client1 = new QuickbloxClient(ApplicationId, AuthorizationKey, AuthorizationSecret, ApiBaseEndPoint, ChatEndpoint, logger);
#if DEBUG
            client1.ChatXmppClient.DebugClientName = "1";
#endif
            await client1.ChatXmppClient.Connect(id1, password1);
            chatManager1 = client1.ChatXmppClient.GetPrivateChatManager(id2, dialogId);
            client1.ChatXmppClient.OnPresenceReceived += (sender, presence) => { if (lastPresencesClient1 != null) lastPresencesClient1.Add(presence); };

            client2 = new QuickbloxClient(ApplicationId, AuthorizationKey, AuthorizationSecret, ApiBaseEndPoint, ChatEndpoint, logger);
#if DEBUG
            client2.ChatXmppClient.DebugClientName = "2";
#endif
            await client2.ChatXmppClient.Connect(id2, password2);
            chatManager2 = client2.ChatXmppClient.GetPrivateChatManager(id1, dialogId);
            client2.ChatXmppClient.OnMessageReceived += (sender, message) => lastMessageClient2 = message;
            client2.ChatXmppClient.OnPresenceReceived += (sender, presence) => { if(lastPresencesClient2 != null) lastPresencesClient2.Add(presence); };
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
        public async Task NotifyIsTypingTest()
        {
            bool isTypingReceived = false;

            chatManager2.OnIsTyping += (obj, e) => isTypingReceived = true;
            chatManager1.NotifyIsTyping();

            await Task.Delay(4000);

            if(!isTypingReceived)
                Assert.Fail("IsTyping wasn't received by client2");
        }

        [TestMethod]
        public async Task NotifyPausedTypingTest()
        {
            bool isPausedTypingReceived = false;

            chatManager2.OnPausedTyping += (obj, e) => isPausedTypingReceived = true;
            chatManager1.NotifyPausedTyping();

            await Task.Delay(4000);

            if(!isPausedTypingReceived)
                Assert.Fail("IsPausedTyping wasn't received ty client2");
        }

        [TestMethod]
        public async Task DisconnectTest()
        {
            lastPresencesClient1 = new List<Presence>();

            client2.ChatXmppClient.Disconnect();
            await Task.Delay(5000);

            if (!lastPresencesClient1.Any(p => p.From.Contains(jid2) && p.PresenceType == PresenceType.Unavailable))
                Assert.Fail("Unavalibility wasn't received by client1");
        }

        [TestMethod]
        public async Task SendAttachmentTest()
        {
            var attachment = new Attachment();
            attachment.Id = "546464654";
            attachment.Name = "Test";
            attachment.Url = "http://image.com/3242423.jpg";
            chatManager2.OnMessageReceived += (sender, message) =>
            {
                Debug.WriteLine("Attachment was received");
            };

            chatManager1.SendAttachemnt(attachment);

            await Task.Delay(5000);
        }
    }
}
