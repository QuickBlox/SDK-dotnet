using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.NotificationModule.Models;
using Quickblox.Sdk.Modules.NotificationModule.Requests;
using Quickblox.Sdk.Modules.NotificationModule.Response;
using Environment = Quickblox.Sdk.Modules.NotificationModule.Models.Environment;

namespace Quickblox.Sdk.Modules.NotificationModule
{
    public class NotificationClient
    {
        private readonly QuickbloxClient quickbloxClient;

        public NotificationClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        public async Task<HttpResponse<CreatePushTokenResponse>> CreatePushToken(Environment environment, String clientIdentificationSequence, Platform platform, String uid)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = new CreatePushTokenRequest();
            settings.Device = new Device() { Platform = platform, Udid = uid };
            settings.PushToken = new PushToken() { Environment = environment, ClientIdentificationSequence = clientIdentificationSequence };

            var resultPushTokenResponse = await HttpService.PostAsync<CreatePushTokenResponse, CreatePushTokenRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SessionMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse> DeletePushToken(String pushTokenId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var methodUrl = String.Format(QuickbloxMethods.DeletePushTokenMethod, pushTokenId);
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, methodUrl, 
                                                                                new NewtonsoftJsonSerializer(), 
                                                                                RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<CreateSubscriptionResponse>> CreateSubscriptions(NotificationChannelType type)
        {
            this.quickbloxClient.CheckIsInitialized();
            var createSubscriptions = new CreateSubscriptionsRequest();
            createSubscriptions.Channel = new NotificationChannel() { Name = type };
            var resultSubscriptionResponse = await HttpService.PostAsync<CreateSubscriptionResponse, CreateSubscriptionsRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        createSubscriptions,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }

        public async Task<HttpResponse<CreateSubscriptionResponse>> GetSubscriptions()
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultSubscriptionResponse = await HttpService.GetAsync<CreateSubscriptionResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }

        public async Task<HttpResponse> DeleteSubscriptions(String subscriptionId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteSubscriptionsMethod, subscriptionId);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, 
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }

        // TODO: не доделан
        public async Task<HttpResponse<CreateEventResponse>> CreateEvent(NotificationType notificationChannelType, Environment environment)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = new CreateEventRequest();
            settings.Event = new CreateEvent();
            settings.Event.NotificationType = notificationChannelType;
            settings.Event.Environment = environment;

            var resultPushTokenResponse = await HttpService.PostAsync<CreateEventResponse, CreateEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEvents()
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.GetAsync<GetSubscriptionsResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<CreateEventResponse>> EditEvent(String name, String message, Boolean isActive = true)
        {
            this.quickbloxClient.CheckIsInitialized();
            var settings = new CreateEventRequest();
            settings.Event = new CreateEvent() {Message = message, EventName = name};
            var resultPushTokenResponse = await HttpService.PostAsync<CreateEventResponse, CreateEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        settings,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse> DeleteEvent(String eventId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteEventMethod, eventId);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }


    }
}
