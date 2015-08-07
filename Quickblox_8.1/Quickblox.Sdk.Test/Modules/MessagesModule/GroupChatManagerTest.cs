using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Message = Quickblox.Sdk.Modules.MessagesModule.Models.Message;

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
        private static QuickbloxClient client1;

        private static string email2 = "to2@test.com";
        private static string password2 = "12345678";
        private static int id2 = 3323883;
        private static string jid2 = "3323883-21183@chat.quickblox.com";
        private static QuickbloxClient client2;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            client1 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            var sessionResponse = await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
            client1.Token = sessionResponse.Result.Session.Token;
#if DEBUG
            client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect(chatEndpoint, id1, (int)GlobalConstant.ApplicationId, password1);

            client2 = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint, new HmacSha1CryptographicProvider());
            var sessionResponse1 = await client2.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email2, password2);
            client2.Token = sessionResponse1.Result.Session.Token;
#if DEBUG
            client2.MessagesClient.DebugClientName = "2";
#endif
            await client2.MessagesClient.Connect(chatEndpoint, id2, (int)GlobalConstant.ApplicationId, password2);
        }

        [TestMethod]
        public async Task SendMessageTest()
        {
            Message lastClient2Message = null;

            var createDialogResponse = await client1.ChatClient.CreateDialogAsync("TestDialog1", DialogType.Group, string.Format("{0},{1}", id2, "3125358"));
            Assert.AreEqual(createDialogResponse.StatusCode, HttpStatusCode.Created);

            IGroupChatManager chatManager1 = client1.MessagesClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid, createDialogResponse.Result.Id);
            IGroupChatManager chatManager2 = client2.MessagesClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid, createDialogResponse.Result.Id);

            chatManager1.JoinGroup(id1.ToString());
            chatManager2.JoinGroup(id2.ToString());

            await Task.Delay(3000);

            string messageText = "Group test message";

            chatManager2.SendMessage(messageText);

            await Task.Delay(5000);
        }


    }
}
