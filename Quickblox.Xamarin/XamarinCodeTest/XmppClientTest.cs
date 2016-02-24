using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quickblox.Sdk;
using Quickblox.Sdk.Test;
using System.Net;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.ChatXmppModule;
using System;

namespace XamarinCodeTest
{
    [TestClass]
    public class XmppClientTest
    {
        private const uint ApplicationId = GlobalConstant.ApplicationId;
        private const string AuthorizationKey = GlobalConstant.AuthorizationKey;
        private const string AuthorizationSecret = GlobalConstant.AuthorizationSecret;
        private const string Login1 = "user1@test.com";
        private const string Password1 = "12345678";

        private const string Login2 = "user2@test.com";
        private const string Password2 = "12345678";

        private QuickbloxClient client;
        private QuickbloxClient client2;

        [TestInitialize]
        public void TestInitialize()
        {
            client = new QuickbloxClient((int)ApplicationId, AuthorizationKey, AuthorizationSecret, GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint);
            client2 = new QuickbloxClient((int)ApplicationId, AuthorizationKey, AuthorizationSecret, GlobalConstant.ApiBaseEndPoint, GlobalConstant.ChatEndpoint);
        }

        [TestMethod]
        public async Task LoginTestMethod()
        {
            var response = await client.AuthenticationClient.CreateSessionWithLoginAsync(Login1, Password1);
            var response2 = await client2.AuthenticationClient.CreateSessionWithLoginAsync(Login2, Password2);

            client.ChatXmppClient.ChatStateChanged += OnStateChanged;
            client.ChatXmppClient.Connect(response.Result.Session.UserId, Password1);
            client2.ChatXmppClient.ChatStateChanged += OnStateChanged2;
            client2.ChatXmppClient.Connect(response2.Result.Session.UserId, Password2);
            //Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
        }

        private void OnStateChanged(object sender, ChatStateChangedEventArgs chatStateChangedEventArgs)
        {
        }

        private void OnStateChanged2(object sender, ChatStateChangedEventArgs chatStateChangedEventArgs)
        {
        }
    }
}
