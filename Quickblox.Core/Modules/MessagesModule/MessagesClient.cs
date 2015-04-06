using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.GeneralDataModel;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Quickblox.Sdk.Modules.MessagesModule.Requests;
using Quickblox.Sdk.Modules.MessagesModule.Response;
using Environment = Quickblox.Sdk.Modules.MessagesModule.Models.Environment;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        private readonly QuickbloxClient quickbloxClient;

        public MessagesClient(QuickbloxClient client)
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

        public async Task<HttpResponse<PagedResponse<EventItem>>> GetEvents()
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.GetAsync<PagedResponse<EventItem>>(this.quickbloxClient.ApiEndPoint,
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
