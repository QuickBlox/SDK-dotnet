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
    /// Push and email notifications client 
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
        internal NotificationClient(QuickbloxClient client)
        {
            this.quickbloxClient = client;
        }
        
        /// <summary>
        /// Сreate device based subscriptions. The authorization token should contain the device parameters. If the subscription is creating for the windows phone pushes make sure that Microsoft Push Notifications have a status "enabled" in the Web Administration Panel.
        /// </summary>
        /// <param name="createSubscriptionsRequest">Parameter for subscription request</param>
        /// <returns>Success HTTP Status Code 201</returns>
        public async Task<HttpResponse<CreateSubscriptionResponseItem[]>> CreateSubscriptionsAsync(CreateSubscriptionsRequest createSubscriptionsRequest)
        {

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            var resultSubscriptionResponse = await HttpService.PostAsync<CreateSubscriptionResponseItem[], CreateSubscriptionsRequest>(this.quickbloxClient.ApiEndPoint,
                                                                                                        QuickbloxMethods.SubscriptionsMethod,
                                                                                                        createSubscriptionsRequest,
                                                                                                        headers);
            return resultSubscriptionResponse;
        }

        /// <summary>
        /// Retrieve subscriptions for the device which is specified in the authorization token.
        /// </summary>
        /// <returns>Success HTTP Status Code 200</returns>
        public async Task<HttpResponse<GetSubscriptionResponse[]>> GetSubscriptionsAsync()
        {
            
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
        public async Task<HttpResponse> DeleteSubscriptionsAsync(Int32 subscriptionId)
        {
            
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
        public async Task<HttpResponse<EventResponse>> CreateEventAsync(CreateEventRequest сreateEventRequest)
        {
            
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
        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEventsAsync()
        {
            
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
        public async Task<HttpResponse<GetSubscriptionsResponse>> GetEventByIdAsync(UInt32 eventId)
        {
            
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
        public async Task<HttpResponse<EventResponse>> EditEventAsync(UInt32 eventId, EditEventRequest editEventRequest)
        {
            
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
        public async Task<HttpResponse> DeleteEventAsync(UInt32 eventId)
        {
            
            var uriMethod = String.Format(QuickbloxMethods.DeleteEventMethod, eventId);
            var resultSubscriptionResponse = await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                                                                                                        uriMethod,
                                                                                                        RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token));
            return resultSubscriptionResponse;
        }
    }
}
