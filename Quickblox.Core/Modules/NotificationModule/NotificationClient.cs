using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.GeneralDataModel.Response;
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

        public async Task<HttpResponse<CreatePushTokenResponse>> CreatePushToken(CreatePushTokenRequest сreatePushTokenRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultPushTokenResponse = await HttpService.PostAsync<CreatePushTokenResponse, CreatePushTokenRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.PushTokenMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        сreatePushTokenRequest,
                                                                                                        headers);

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse> DeletePushToken(String pushTokenId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var methodUrl = String.Format(QuickbloxMethods.DeletePushTokenMethod, pushTokenId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultPushTokenResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, methodUrl, 
                                                                                new NewtonsoftJsonSerializer(),
                                                                                headers);

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse> CreateSubscriptions(NotificationChannelType type)
        {
            this.quickbloxClient.CheckIsInitialized();
            var createSubscriptions = new CreateSubscriptionsRequest {Name = type};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.PostAsync<Object, CreateSubscriptionsRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        createSubscriptions,
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        public async Task<HttpResponse<GetSubscriptionResponse[]>> GetSubscriptions()
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.GetAsync<GetSubscriptionResponse[]>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        public async Task<HttpResponse> DeleteSubscriptions(Int32 subscriptionId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteSubscriptionsMethod, subscriptionId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, 
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        public async Task<HttpResponse<EventResponse>> CreateEvent(CreateEventRequest сreateEventRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.PostAsync<EventResponse, CreateEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        сreateEventRequest,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEvents()
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.GetAsync<GetSubscriptionsResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventsMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEventById(UInt32 eventId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = string.Format(QuickbloxMethods.GetEventByIdMethod, eventId);
            var resultPushTokenResponse = await HttpService.GetAsync<GetSubscriptionsResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse<EventResponse>> EditEvent(UInt32 eventId, EditEventRequest editEventRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.EditEventMethod, eventId);
            var resultPushTokenResponse = await HttpService.PutAsync<EventResponse, EditEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        new NewtonsoftJsonSerializer(),
                                                                                                        editEventRequest,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        public async Task<HttpResponse> DeleteEvent(UInt32 eventId)
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
