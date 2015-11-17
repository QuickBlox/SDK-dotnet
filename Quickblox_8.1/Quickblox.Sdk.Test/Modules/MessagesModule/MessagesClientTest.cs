using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;

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
            client1 = new QuickbloxClient((int)GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint);
            //await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
#if DEBUG
            //client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect(id1, password1);
        }

        [TestMethod]
        public async Task GetContactsTest()
        {
            bool contactsChanged = false;
            client1.MessagesClient.OnContactsChanged += (sender, args) => contactsChanged = true;

            client1.MessagesClient.ReloadContacts();

            await Task.Delay(10000);

            if(!contactsChanged)
                Assert.Fail("Contacts were not received");

#if DEBUG
            Debug.WriteLine("Roster items received from server:");
            foreach (var contact in client1.MessagesClient.Contacts)
            {
                Debug.WriteLine("ID: {0} Name: {1}", contact.UserId, contact.Name);
            }
#endif
        }

        [TestMethod]
        public async Task AddContactTest()
        {
            var testContact = new Contact {Name = "Test Contact", UserId = 2701450};

            client1.MessagesClient.AddContact(testContact);
            await Task.Delay(10000);

            var contact = client1.MessagesClient.Contacts.FirstOrDefault(c => c.Name == testContact.Name && c.UserId == testContact.UserId);
            if(contact == null)
                Assert.Fail("Contact wasn't added to contact list.");
        }

        [TestMethod]
        public async Task DeleteContactTest()
        {
            var testContact = new Contact { Name = "Test Contact", UserId = 2701450 };

            client1.MessagesClient.AddContact(testContact);
            await Task.Delay(10000);

            client1.MessagesClient.DeleteContact(testContact.UserId);
            await Task.Delay(10000);

            var contact = client1.MessagesClient.Contacts.FirstOrDefault(c => c.Name == testContact.Name && c.UserId == testContact.UserId);
            if (contact != null)
                Assert.Fail("Contact wasn't removed from contact list.");
        }
    }
}
