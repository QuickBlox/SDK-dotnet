using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class GroupChatManagerTest
    {
        private static string groupJid = "21183_5582c4f76390d8b9e901e6e2@muc.chat.quickblox.com";
        private static string chatEndpoint = "chat.quickblox.com";

        private static string email1 = "to1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 3323859;
        private static string jid1 = "3323859-21183@chat.quickblox.com";

        private static string email2 = "to2@test.com";
        private static string password2 = "12345678";
        private static int id2 = 3323883;
        private static string jid2 = "3323883-21183@chat.quickblox.com";

        private static QuickbloxClient client1;
        private static IGroupChatManager chatManager1;

        private static QuickbloxClient client2;
        private static IGroupChatManager chatManager2;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            client1 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            //await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
#if DEBUG
            client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect(chatEndpoint, id1, (int)GlobalConstant.ApplicationId, password1);
            chatManager1 = client1.MessagesClient.GetGroupChatManager(groupJid);

            client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            //await client2.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client2.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email2, password2);
#if DEBUG
            client2.MessagesClient.DebugClientName = "2";
#endif
            await client2.MessagesClient.Connect(chatEndpoint, id2, (int)GlobalConstant.ApplicationId, password2);
            chatManager2 = client2.MessagesClient.GetGroupChatManager(groupJid);
        }

        [TestMethod]
        public async Task SendMessageTest()
        {
            Message lastClient2Message = null;

            chatManager1.JoinGroup("User1");
            chatManager2.JoinGroup("User2");
            await Task.Delay(1000);

            string messageText = "Group test message";

            chatManager1.RequestVoice();

            await Task.Delay(2000);
            chatManager1.SendMessage(messageText);
            await Task.Delay(5000);
            
        }


    }
}
