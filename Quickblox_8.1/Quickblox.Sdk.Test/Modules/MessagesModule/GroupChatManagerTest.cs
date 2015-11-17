﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatXmppModule.Interfaces;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class GroupChatManagerTest
    {
        private static string apiEndpoint = "https://api.quickblox.com";
        private static string chatEndpoint = "chat.quickblox.com";
        private static uint appId = 13318;
        private static string authKey = "WzrAY7vrGmbgFfP";
        private static string authSecret = "xS2uerEveGHmEun";

        private static string groupJid = "13318_560befeca28f9a20170005f4@muc.chat.quickblox.com";
        private static string groupDialogId = "560befeca28f9a20170005f4";

        private static string email1 = "user1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 5719149;
        private static string jid1 = "5719149-13318@chat.quickblox.com";
        private static QuickbloxClient client1;

        private static string email2 = "user2@test.com";
        private static string password2 = "12345678";
        private static int id2 = 5513419;
        private static string jid2 = "5513419-13318@chat.quickblox.com";
        private static QuickbloxClient client2;

        private static string email3 = "user3@test.com";
        private static string password3 = "12345678";
        private static int id3 = 5513474;
        private static string jid3 = "5513474-13318@chat.quickblox.com";
        private static QuickbloxClient client3;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            var logger = new Logger.DebugLogger();

            client1 = new QuickbloxClient(apiEndpoint, chatEndpoint, logger);
            var sessionResponse = await client1.CoreClient.CreateSessionWithEmailAsync(appId, authKey, authSecret, email1, password1);
            client1.Token = sessionResponse.Result.Session.Token;
#if DEBUG
            client1.ChatXmppClient.DebugClientName = "1";
#endif
            await client1.ChatXmppClient.Connect(chatEndpoint, id1, (int)appId, password1);

            client2 = new QuickbloxClient(apiEndpoint, chatEndpoint, logger);
            var sessionResponse2 = await client2.CoreClient.CreateSessionWithEmailAsync(appId, authKey, authSecret, email2, password2);
            client2.Token = sessionResponse2.Result.Session.Token;
#if DEBUG
            client2.ChatXmppClient.DebugClientName = "2";
#endif
            await client2.ChatXmppClient.Connect(chatEndpoint, id2, (int)appId, password2);

            client3 = new QuickbloxClient(apiEndpoint, chatEndpoint, logger);
            var sessionResponse3 = await client2.CoreClient.CreateSessionWithEmailAsync(appId, authKey, authSecret, email2, password2);
            client3.Token = sessionResponse3.Result.Session.Token;
#if DEBUG
            client3.ChatXmppClient.DebugClientName = "3";
#endif
            await client3.ChatXmppClient.Connect(chatEndpoint, id3, (int)appId, password3);
        }

        [TestMethod]
        public async Task SendMessageTest()
        {
            Message lastClient2Message = null;

            var createDialogResponse = await client1.ChatClient.CreateDialogAsync("TestDialog1", DialogType.Group, string.Format("{0},{1}", id2, "3125358"));
            Assert.AreEqual(createDialogResponse.StatusCode, HttpStatusCode.Created);

            IGroupChatManager chatManager1 = client1.ChatXmppClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid, createDialogResponse.Result.Id);
            IGroupChatManager chatManager2 = client2.ChatXmppClient.GetGroupChatManager(createDialogResponse.Result.XmppRoomJid, createDialogResponse.Result.Id);

            chatManager1.JoinGroup(id1.ToString());
            chatManager2.JoinGroup(id2.ToString());

            await Task.Delay(3000);

            string messageText = "Group test message";

            chatManager2.SendMessage(messageText);

            await Task.Delay(5000);
        }

        [TestMethod]
        public async Task GroupChatTest()
        {
            IGroupChatManager chatManager1 = client1.ChatXmppClient.GetGroupChatManager(groupJid, groupDialogId);
            chatManager1.JoinGroup(id1.ToString());
            IGroupChatManager chatManager2 = client2.ChatXmppClient.GetGroupChatManager(groupJid, groupDialogId);
            chatManager2.JoinGroup(id2.ToString());
            IGroupChatManager chatManager3 = client3.ChatXmppClient.GetGroupChatManager(groupJid, groupDialogId);
            chatManager3.JoinGroup(id3.ToString());
            await Task.Delay(2000);

            chatManager1.SendMessage("One reports to the group.");
            await Task.Delay(1000);
            chatManager2.SendMessage("Three reports to the group");
            await Task.Delay(1000);
            chatManager3.SendMessage("Three reports to the group");
            await Task.Delay(1000);

            Debug.WriteLine("############ Updating name");
            string newName = "Name1";
            var updateDialogRequest = new UpdateDialogRequest { DialogId = groupDialogId };
            updateDialogRequest.Name = newName;
            var updateDialogResponse = await client2.ChatClient.UpdateDialogAsync(updateDialogRequest);
            Assert.AreEqual(updateDialogResponse.StatusCode, HttpStatusCode.OK);

            chatManager2.NotifyGroupNameChanged(newName);
            await Task.Delay(1000);

            Debug.WriteLine("############ Checking messaging again");

            chatManager1.SendMessage("One reports to the group.");
            await Task.Delay(1000);
            chatManager2.SendMessage("Three reports to the group");
            await Task.Delay(1000);
            chatManager3.SendMessage("Three reports to the group");
            await Task.Delay(1000);

            await Task.Delay(3000);
        }


    }
}
