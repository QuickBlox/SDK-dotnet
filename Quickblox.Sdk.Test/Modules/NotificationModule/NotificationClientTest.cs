using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Hmacsha;
using Quickblox.Sdk.Modules.NotificationModule.Models;
using Quickblox.Sdk.Modules.NotificationModule.Requests;
using System.Net;
using System;
using System.Linq;
using System.Text;
using Windows.Foundation.Metadata;
using Windows.Networking.PushNotifications;
using Windows.System.Profile;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Test.Helper;
using Environment = Quickblox.Sdk.Modules.NotificationModule.Models.Environment;
using Platform = Quickblox.Sdk.GeneralDataModel.Models.Platform;

namespace Quickblox.Sdk.Test.Modules.NotificationModule
{
    [TestClass]
    public class NotificationClientTest
    {
        private QuickbloxClient client;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this.client = new QuickbloxClient(GlobalConstant.ApiBaseEndPoint, GlobalConstant.AccountKey, new HmacSha1CryptographicProvider());
            await this.client.InitializeClient();
            await this.client.CoreClient.CreateSessionWithLogin(GlobalConstant.ApplicationId, GlobalConstant.AuthorizationKey, GlobalConstant.AuthorizationSecret, "Test654321", "Test12345", deviceRequestRequest: new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() });
        }
        

        [TestMethod]
        public async Task CreatePushTokenSuccessTest()
        {
            var pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var settings = new CreatePushTokenRequest()
            {
                DeviceRequest = new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() },
                PushToken = new PushToken() { Environment = Environment.production, ClientIdentificationSequence = pushChannel.Uri }
            };
            var createPushTokenResponse = await this.client.NotificationClient.CreatePushToken(settings);
            Assert.AreEqual(createPushTokenResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task DeletePushTokenSuccessTest()
        {
            var pushChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var settings = new CreatePushTokenRequest()
            {
                DeviceRequest = new DeviceRequest() { Platform = Platform.windows_phone, Udid = Helpers.GetHardwareId() },
                PushToken = new PushToken() { Environment = Environment.production, ClientIdentificationSequence = pushChannel.Uri }
            };
            var createPushTokenResponse = await this.client.NotificationClient.CreatePushToken(settings);
            Assert.AreEqual(createPushTokenResponse.StatusCode, HttpStatusCode.Created);

            var response = await this.client.NotificationClient.DeletePushToken(createPushTokenResponse.Result.PushToken.PushTokenId);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreateSubscriptionsSuccessTest()
        {
            var createSubscriptionsResponse = await this.client.NotificationClient.CreateSubscriptions(NotificationChannelType.mpns);
            Assert.AreEqual(createSubscriptionsResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetSubscriptionsSuccessTest()
        {
            var createSubscriptionsResponse = await this.client.NotificationClient.GetSubscriptions();
            Assert.AreEqual(createSubscriptionsResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSubscriptionsSuccessTest()
        {
            var createSubscriptionsResponse = await this.client.NotificationClient.GetSubscriptions();
            Assert.AreEqual(createSubscriptionsResponse.StatusCode, HttpStatusCode.OK);

            var deleteResponse = await this.client.NotificationClient.DeleteSubscriptions(createSubscriptionsResponse.Result.First().Subscription.Id);
            Assert.AreEqual(deleteResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreateEventFullSuccessTest()
        {
            var createEventRequest = new CreateEventRequest();
            createEventRequest.Event = new CreateEvent()
            {
                NotificationType = NotificationType.push,
                Environment = Environment.production,
                PushType = PushType.mpns,
                Message = new PushMessage("Title", Convert.ToBase64String(Encoding.UTF8.GetBytes("I love quickblox"))),
                User = new UserWithTags() {  Ids = "2701456" }
            };
            var createEventResponse = await this.client.NotificationClient.CreateEvent(createEventRequest);
            Assert.AreEqual(createEventResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task CreateEventSimpleSuccessTest()
        {
            var createEventRequest = new CreateEventRequest();
            createEventRequest.Event = new CreateEvent()
            {
                NotificationType = NotificationType.push,
                Environment = Environment.production,
                PushType = PushType.mpns,
                Message = new SimplePushMessage("I love quickblox"),
                User = new UserWithTags() { Ids = "2701456" }
            };
            var createEventResponse = await this.client.NotificationClient.CreateEvent(createEventRequest);
            Assert.AreEqual(createEventResponse.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetEventsSuccessTest()
        {
            var getEventsResponse = await this.client.NotificationClient.GetEvents();
            Assert.AreEqual(getEventsResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetEventsByIdSuccessTest()
        {
            var getEventsResponse = await this.client.NotificationClient.GetEvents();
            Assert.AreEqual(getEventsResponse.StatusCode, HttpStatusCode.OK);

            UInt32 testEventId = getEventsResponse.Result.Items.First().Event.Id;

            var getEventByIdResponse = await this.client.NotificationClient.GetEventById(testEventId);
            Assert.AreEqual(getEventByIdResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task EditEventSuccessTest()
        {
            var getEventsResponse = await this.client.NotificationClient.GetEvents();
            Assert.AreEqual(getEventsResponse.StatusCode, HttpStatusCode.OK);

            var editEventRequest = new EditEventRequest()
            {
                EditEvent = new EditEvent()
                {
                    Message = new PushMessage("Title", "I love quickblox 3"),
                }
            };

            UInt32 eventId = getEventsResponse.Result.Items.First().Event.Id;
            var editEventsResponse = await this.client.NotificationClient.EditEvent(eventId, editEventRequest);
            Assert.AreEqual(editEventsResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteEventSuccessTest()
        {
            var getEventsResponse = await this.client.NotificationClient.GetEvents();
            Assert.AreEqual(getEventsResponse.StatusCode, HttpStatusCode.OK);

            UInt32 testEventId = getEventsResponse.Result.Items.First().Event.Id;
            var deleteEventResponse = await this.client.NotificationClient.DeleteEvent(testEventId);
            Assert.AreEqual(deleteEventResponse.StatusCode, HttpStatusCode.OK);
        }
    }
}
