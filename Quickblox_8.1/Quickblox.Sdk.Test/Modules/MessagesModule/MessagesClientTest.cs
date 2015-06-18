using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class MessagesClientTest
    {
        private static string chatEndpoint = "chat.quickblox.com";

        private static string email1 = "to1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 3323859;
        private static string jid1 = "3323859-21183@chat.quickblox.com";

        private static QuickbloxClient client1;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            client1 = new QuickbloxClient();
            //await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
#if DEBUG
            client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect(chatEndpoint, id1, (int)GlobalConstant.ApplicationId, password1);
        }

        [TestMethod]
        public async Task GetContactsTest()
        {
            bool contactsChanged = false;

            client1.MessagesClient.OnContactsChanged += (sender, args) => contactsChanged = true;
            client1.MessagesClient.ReloadContacts();

            await Task.Delay(10000);

            if(!contactsChanged)
                Assert.Fail("Contacts wasn't received");
        }

        [TestMethod]
        public async Task AddContactTest()
        {
            client1.MessagesClient.AddContact(new Contact() { Name = "Test Contact", UserId = 2701450 });
            await Task.Delay(10000);
        }

        [TestMethod]
        public async Task DeleteContactTest()
        {
            client1.MessagesClient.DeleteContact(2701450);
            await Task.Delay(10000);
        }
    }
}
