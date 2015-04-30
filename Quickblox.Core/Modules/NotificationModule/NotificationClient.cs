using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.NotificationModule.Models;
using Quickblox.Sdk.Modules.NotificationModule.Requests;
using Quickblox.Sdk.Modules.NotificationModule.Responses;

namespace Quickblox.Sdk.Modules.NotificationModule
{
    /// <summary>
    /// Client present API for push
    /// http://quickblox.com/developers/Messages
    /// </summary>
    public class NotificationClient
    {
        /// <summary>
        /// The quickblox client
        /// </summary>
        private readonly QuickbloxClient quickbloxClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public NotificationClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }

        /// <summary>
        /// Create push token (Token for iOS, Registration Id for Android, Uri for Windows Phone). Neеd to get the authorization token with the device parameters (platform, udid).
        /// </summary>
        /// <param name="сreatePushTokenRequest">The сreate push token request.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<CreatePushTokenResponse>> CreatePushToken(CreatePushTokenRequest сreatePushTokenRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultPushTokenResponse = await HttpService.PostAsync<CreatePushTokenResponse, CreatePushTokenRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.PushTokenMethod,
                                                                                                        сreatePushTokenRequest,
                                                                                                        headers);

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Delete push token by identifier.
        /// </summary>
        /// <param name="pushTokenId">The push token identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> DeletePushToken(String pushTokenId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var methodUrl = String.Format(QuickbloxMethods.DeletePushTokenMethod, pushTokenId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultPushTokenResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, methodUrl, 
                                                                                headers);

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Сreate device based subscriptions. The authorization token should contain the device parameters. If the subscription is creating for the windows phone pushes make sure that Microsoft Push Notifications have a status "enabled" in the Web Administration Panel.
        /// </summary>
        /// <param name="type">Notification channel type.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse> CreateSubscriptions(NotificationChannelType type)
        {
            this.quickbloxClient.CheckIsInitialized();
            var createSubscriptions = new CreateSubscriptionsRequest {Name = type};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.PostAsync<Object, CreateSubscriptionsRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        createSubscriptions,
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        /// <summary>
        /// Retrieve subscriptions for the device which is specified in the authorization token.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GetSubscriptionResponse[]>> GetSubscriptions()
        {
            this.quickbloxClient.CheckIsInitialized();
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.GetAsync<GetSubscriptionResponse[]>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        /// <summary>
        /// Remove a subscription by the identifier.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> DeleteSubscriptions(Int32 subscriptionId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteSubscriptionsMethod, subscriptionId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, 
                                                                                                        uriMethod,
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        /// <summary>
        /// Create notification event
        /// </summary>
        /// <param name="сreateEventRequest">The сreate event parameter.</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<EventResponse>> CreateEvent(CreateEventRequest сreateEventRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.PostAsync<EventResponse, CreateEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventsMethod,
                                                                                                        сreateEventRequest,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Get all events which were created by a user specified in the authorization token.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEvents()
        {
            this.quickbloxClient.CheckIsInitialized();
            var resultPushTokenResponse = await HttpService.GetAsync<GetSubscriptionsResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.EventsMethod,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Retrieve event by the ID. The event specified in the request should belong to the application for which the authorization token has been received.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEventById(UInt32 eventId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = string.Format(QuickbloxMethods.GetEventByIdMethod, eventId);
            var resultPushTokenResponse = await HttpService.GetAsync<GetSubscriptionsResponse>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Edit event by ID.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="editEventRequest">The edit event parameter.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<EventResponse>> EditEvent(UInt32 eventId, EditEventRequest editEventRequest)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.EditEventMethod, eventId);
            var resultPushTokenResponse = await HttpService.PutAsync<EventResponse, EditEventRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        editEventRequest,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));

            return resultPushTokenResponse;
        }

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse> DeleteEvent(UInt32 eventId)
        {
            this.quickbloxClient.CheckIsInitialized();
            var uriMethod = String.Format(QuickbloxMethods.DeleteEventMethod, eventId);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }
    }
}
