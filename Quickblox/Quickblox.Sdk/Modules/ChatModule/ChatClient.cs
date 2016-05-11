using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Filters;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatModule.Responses;
using Quickblox.Sdk.Modules.Models;

namespace Quickblox.Sdk.Modules.ChatModule
{
    /// <summary>
    /// Chat module allows to manage user dialogs.
    /// http://quickblox.com/developers/Chat
    /// </summary>
    public class ChatClient
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        internal ChatClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods

        #region Dialogs

        /// <summary>
        /// Creates a new dialog.
        /// </summary>
        /// <param name="dialogName">Dialog name. Is ignored for Private dialogs.</param>
        /// <param name="dialogType">Dialog type</param>
        /// <param name="occupantsIds">Occupants IDs (in a string separated by comma)</param>
        /// <param name="photoId">Photo upload ID.</param>
        /// <returns></returns>
        public async Task<HttpResponse<Dialog>> CreateDialogAsync(string dialogName, DialogType dialogType, string occupantsIds, string photoId = null)
        {
            var createDialogRequest = new CreateDialogRequest {Type = (int) dialogType, Name = dialogName, OccupantsIds = occupantsIds, Photo = photoId};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<Dialog, CreateDialogRequest>(this.quickbloxClient.ApiEndPoint,
                QuickbloxMethods.CreateDialogMethod, createDialogRequest, headers);
        }

        /// <summary>
        /// Returns all dialogs associated with current user
        /// </summary>
        /// <param name="retrieveDialogsRequest">Retrieve dialogs request info</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveDialogsResponse>> GetDialogsAsync(RetrieveDialogsRequest retrieveDialogsRequest = null)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveDialogsResponse, RetrieveDialogsRequest>(this.quickbloxClient.ApiEndPoint,
                QuickbloxMethods.GetDialogsMethod, retrieveDialogsRequest, headers);
        }

        /// <summary>
        /// Updates a dialog. Works only if type=1(PUBLIC_GROUP) or 2(GROUP). 
        /// Users who are in occupants_ids can update a dialog with type=2(GROUP). If type=1(PUBLIC_GROUP) - only dialog’s owner can update it. 
        /// </summary>
        /// <param name="updateDialogRequest">Update dialog request info</param>
        /// <returns></returns>
        public async Task<HttpResponse<Dialog>> UpdateDialogAsync(UpdateDialogRequest updateDialogRequest)
        {
            if (updateDialogRequest == null)
                throw new ArgumentNullException("updateDialogRequest");

            var uriMethod = String.Format(QuickbloxMethods.UpdateDialogMethod, updateDialogRequest.DialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<Dialog, UpdateDialogRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, updateDialogRequest, headers);
        }

        /// <summary>
        /// Deletes chat dialog. Each user from dialog’s occupant_ids field can remove the dialog.
        /// This doesn’t mean that this dialog will be removed completely for all the users in this dialog. It will be removed only for current user. 
        /// To completely remove a dialog - pass force=1. Only owner can do it.
        /// </summary>
        /// <param name="dialogId">Diglod ID to be removed</param>
        /// <returns></returns>
        public async Task<HttpResponse<Object>> DeleteDialogAsync(string dialogId)
        {
            if (dialogId == null)
                throw new ArgumentNullException("dialogId");

            var uriethod = String.Format(QuickbloxMethods.DeleteDialogMethod, dialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                uriethod, headers);
        }

        #endregion

        #region Messages

        /// <summary>
        /// Creates a chat Message. It’s possible to inject a new chat Message to the chat history. In this case this new Message won't be delivered to the recipient(s) by XMPP real time transport, it will be just added to the history. If you wont to initiates a real 'send to chat' - pass send_to_chat=1 parameter.
        /// </summary>
        /// <param name="createMessageRequest">Create Message request info</param>
        /// <returns></returns>
        public async Task<HttpResponse<CreateMessageResponse>> CreateMessageAsync(CreateMessageRequest createMessageRequest)
        {
            if (createMessageRequest == null)
                throw new ArgumentNullException("createMessageRequest");

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateMessageResponse, CreateMessageRequest>(this.quickbloxClient.ApiEndPoint,
                QuickbloxMethods.CreateMessageMethod, createMessageRequest, headers);
        }

        /// <summary>
        /// Retrieves all chat messages within particular dialog. It's only possible to read chat messages in dialog if current user id is in occupants_ids field or if dialog's type=1(PUBLIC_GROUP). Server will return dialog's chat messages sorted ascending by date_sent field. 
        /// All retrieved chat messages will be marked as read after request. 
        /// </summary>
        /// <param name="dialogId">Dialog ID</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveMessagesResponse>> GetMessagesAsync(string dialogId)
        {
            if (dialogId == null)
                throw new ArgumentNullException("dialogId");

            var retrieveMessagesRequest = new RetrieveMessagesRequest();
            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new FieldFilter<string>(() => new Message().ChatDialogId, dialogId));
            retrieveMessagesRequest.Filter = aggregator;

            return await GetMessagesAsync(retrieveMessagesRequest);
        }

        /// <summary>
        /// Retrieves all chat messages within particular dialog. It's only possible to read chat messages in dialog if current user id is in occupants_ids field or if dialog's type=1(PUBLIC_GROUP). Server will return dialog's chat messages sorted ascending by date_sent field. 
        /// All retrieved chat messages will be marked as read after request. 
        /// </summary>
        /// <param name="retrieveMessagesRequest">Get messages info</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveMessagesResponse>> GetMessagesAsync(RetrieveMessagesRequest retrieveMessagesRequest)
        {
            if (retrieveMessagesRequest == null)
                throw new ArgumentNullException("retrieveMessagesRequest");

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveMessagesResponse, RetrieveMessagesRequest>(this.quickbloxClient.ApiEndPoint,
                QuickbloxMethods.GetMessagesMethod, retrieveMessagesRequest, headers);
        }

        /// <summary>
        /// Retrieves all chat messages within particular dialog. It's only possible to read chat messages in dialog if current user id is in occupants_ids field or if dialog's type=1(PUBLIC_GROUP). Server will return dialog's chat messages sorted ascending by date_sent field. 
        /// All retrieved chat messages will be marked as read after request. 
        /// </summary>
        /// <param name="dialogId">Dialog ID</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveMessagesResponse<T>>> GetMessagesAsync<T>(string dialogId) where T : Message
        {
            if (dialogId == null)
                throw new ArgumentNullException("dialogId");

            var retrieveMessagesRequest = new RetrieveMessagesRequest();
            var aggregator = new FilterAggregator();
            aggregator.Filters.Add(new FieldFilter<string>(() => new Message().ChatDialogId, dialogId));
            retrieveMessagesRequest.Filter = aggregator;

            return await GetMessagesAsync<T>(retrieveMessagesRequest);
        }

        /// <summary>
        /// Retrieves all chat messages within particular dialog. It's only possible to read chat messages in dialog if current user id is in occupants_ids field or if dialog's type=1(PUBLIC_GROUP). Server will return dialog's chat messages sorted ascending by date_sent field. 
        /// All retrieved chat messages will be marked as read after request. 
        /// </summary>
        /// <param name="retrieveMessagesRequest">Get messages info</param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveMessagesResponse<T>>> GetMessagesAsync<T>(RetrieveMessagesRequest retrieveMessagesRequest) where T : Message
        {
            if (retrieveMessagesRequest == null)
                throw new ArgumentNullException("retrieveMessagesRequest");

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveMessagesResponse<T>, RetrieveMessagesRequest>(this.quickbloxClient.ApiEndPoint,
                QuickbloxMethods.GetMessagesMethod, retrieveMessagesRequest, headers);
        }

        /// <summary>
        /// Updates a chat Message.
        /// It's possible to mark all messages as read/delivered - just don't pass a Message id.
        /// </summary>
        /// <param name="updateMessageRequest"></param>
        /// <returns></returns>
        public async Task<HttpResponse<RetrieveMessagesResponse>> UpdateMessageAsync(UpdateMessageRequest updateMessageRequest)
        {
            if (updateMessageRequest == null)
                throw new ArgumentNullException("updateMessageRequest");

            var uriMethod = String.Format(QuickbloxMethods.UpdateMessageMethod, updateMessageRequest.ChatDialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<RetrieveMessagesResponse, UpdateMessageRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, updateMessageRequest, headers);
        }

        /// <summary>
        /// Any user in the dialog’s occupant_ids is able to remove a Message from the dialog. The Message will only be removed for the current user - the Message will still be viewable and in the chat history for all other users in the dialog.
        /// </summary>
        /// <param name="occupantIds"></param>
        /// <returns></returns>
        public async Task<HttpResponse<Object>> DeleteMessageAsync(String[] occupantIds)
        {
            if (occupantIds == null)
                throw new ArgumentNullException("occupantIds");

            var uriMethod = String.Format(QuickbloxMethods.DeleteMessageMethod, String.Join(",", occupantIds));
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
        }

        #endregion

        #endregion

    }
}
