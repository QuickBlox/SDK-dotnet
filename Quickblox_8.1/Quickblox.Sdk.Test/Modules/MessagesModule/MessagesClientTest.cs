using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class MessagesClientTest
    {
        private static string email1 = "to1@test.com";
        private static string password1 = "12345678";
        private static int id1 = 3323859;
        private static string jid1 = "3323859-21183@chat.quickblox.com";

        private static QuickbloxClient client1;

        [TestMethod]
        public async Task RosterTest()
        {
            bool contactsChanged = false;

            client1 = new QuickbloxClient();
            //await client1.InitializeClientAsync(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            //await client1.CoreClient.CreateSessionWithEmailAsync(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, email1, password1);
#if DEBUG
            client1.MessagesClient.DebugClientName = "1";
#endif
            await client1.MessagesClient.Connect("chat.quickblox.com", id1, (int)GlobalConstant.ApplicationId, password1);

            client1.MessagesClient.ReloadContacts();
            client1.MessagesClient.OnContactsChanged += (sender, args) => contactsChanged = true;

            await Task.Delay(10000);

            if(!contactsChanged)
                Assert.Fail("Contacts wasn't received");
        }
    }
}
