using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.Core.Serializer;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatModule.Responses;

namespace Quickblox.Sdk.Modules.ChatModule
{
    public class ChatClient
    {
        #region Fields

        private readonly QuickbloxClient quickbloxClient;

        #endregion


        #region Ctor

        public ChatClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods
        
        public async Task<HttpResponse<CreateDialogResponse>> CreateDialog(string dialogName, DialogType dialogType, string occupantsIds = null, string photoId = null)
        {
            if (dialogName == null)
                throw new ArgumentNullException("dialogName is null");

            var createDialogRequest = new CreateDialogRequest {Type = (int) dialogType, Name = dialogName, OccupantsIds = occupantsIds, Photo = photoId};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateDialogResponse, CreateDialogRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.CreateDialogMethod, new NewtonsoftJsonSerializer(), createDialogRequest, headers);
        }

        public async Task<HttpResponse<CreateMessageResponse>> CreateMessage(CreateMessageRequest createMessageRequest)
        {
            if (createMessageRequest == null)
                throw new ArgumentNullException("createMessageRequest is null");

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateMessageResponse, CreateMessageRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.CreateMessageMethod, new NewtonsoftJsonSerializer(), createMessageRequest, headers);
        }

        // TODO: Add filters
        public async Task<HttpResponse<RetrieveDialogsResponse>> GetDialogs()
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveDialogsResponse>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.GetDialogsMethod, new NewtonsoftJsonSerializer(), headers);
        }

        public async Task<HttpResponse<RetrieveMessagesResponse>> GetMessages(String dialogId)
        {
            if (dialogId == null)
                throw new ArgumentNullException("updateDialogRequest is null");

            var uriMethod = String.Format(QuickbloxMethods.GetMessagesMethod, dialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveMessagesResponse>(this.quickbloxClient.ApiEndPoint,
                         uriMethod, new NewtonsoftJsonSerializer(), headers);
        }

        public async Task<HttpResponse<RetrieveMessagesResponse>> UpdateMessage(UpdateMessageRequest updateDialogRequest)
        {
            if (updateDialogRequest == null)
                throw new ArgumentNullException("updateDialogRequest is null");

            var uriMethod = String.Format(QuickbloxMethods.UpdateMessageMethod, updateDialogRequest.ChatDialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<RetrieveMessagesResponse, UpdateMessageRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, new NewtonsoftJsonSerializer(), updateDialogRequest, headers);
        }

        public async Task<HttpResponse<Object>> DeleteMessage(String[] occupantIds)
        {
            if (occupantIds == null)
                throw new ArgumentNullException("occupantIds is null");

            var uriMethod = String.Format(QuickbloxMethods.DeleteMessageMethod, String.Join(",", occupantIds));
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, uriMethod, new NewtonsoftJsonSerializer(), headers);
        }

        #endregion


    }
}
