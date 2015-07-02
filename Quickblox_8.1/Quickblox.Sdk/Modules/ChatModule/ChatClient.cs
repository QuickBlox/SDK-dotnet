using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Core;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Http;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.ChatModule.Requests;
using Quickblox.Sdk.Modules.ChatModule.Responses;
using Quickblox.Sdk.Modules.Models;

namespace Quickblox.Sdk.Modules.ChatModule
{
    public class ChatClient
    {
        #region Fields

        private readonly IQuickbloxClient quickbloxClient;

        #endregion


        #region Ctor

        public ChatClient(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region Public methods
        
        public async Task<HttpResponse<DialogResponse>> CreateDialogAsync(string dialogName, DialogType dialogType, string occupantsIds = null, string photoId = null)
        {
            if (dialogName == null)
                throw new ArgumentNullException("dialogName");

            var createDialogRequest = new CreateDialogRequest {Type = (int) dialogType, Name = dialogName, OccupantsIds = occupantsIds, Photo = photoId};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<DialogResponse, CreateDialogRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.CreateDialogMethod, createDialogRequest, headers);
        }

        public async Task<HttpResponse<RetrieveDialogsResponse>> GetDialogsAsync(RetrieveDialogsRequest retrieveDialogsRequest = null)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveDialogsResponse, RetrieveDialogsRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.GetDialogsMethod, retrieveDialogsRequest, headers);
        }

        public async Task<HttpResponse<Dialog>> UpdateDialogAsync(UpdateDialogRequest updateDialogRequest)
        {
            if (updateDialogRequest == null)
                throw new ArgumentNullException("updateDialogRequest");

            var uriMethod = String.Format(QuickbloxMethods.UpdateDialogMethod, updateDialogRequest.DialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<Dialog, UpdateDialogRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, updateDialogRequest, headers);
        }

        public async Task<HttpResponse<Object>> DeleteDialogAsync(string dialogId)
        {
            if (dialogId == null)
                throw new ArgumentNullException("dialogId");

            var uriethod = String.Format(QuickbloxMethods.DeleteDialogMethod, dialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint,
                        uriethod, headers);
        }
        
        public async Task<HttpResponse<CreateMessageResponse>> CreateMessageAsync(CreateMessageRequest createMessageRequest)
        {
            if (createMessageRequest == null)
                throw new ArgumentNullException("createMessageRequest");

            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateMessageResponse, CreateMessageRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.CreateMessageMethod, createMessageRequest, headers);
        }

        

        public async Task<HttpResponse<RetrieveMessagesResponse>> GetMessagesAsync(String dialogId)
        {
            if (dialogId == null)
                throw new ArgumentNullException("dialogId");

            var uriMethod = String.Format(QuickbloxMethods.GetMessagesMethod, dialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.GetAsync<RetrieveMessagesResponse>(this.quickbloxClient.ApiEndPoint,
                         uriMethod, headers);
        }

        public async Task<HttpResponse<RetrieveMessagesResponse>> UpdateMessageAsync(UpdateMessageRequest updateMessageRequest)
        {
            if (updateMessageRequest == null)
                throw new ArgumentNullException("updateMessageRequest");

            var uriMethod = String.Format(QuickbloxMethods.UpdateMessageMethod, updateMessageRequest.ChatDialogId);
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PutAsync<RetrieveMessagesResponse, UpdateMessageRequest>(this.quickbloxClient.ApiEndPoint, uriMethod, updateMessageRequest, headers);
        }

        public async Task<HttpResponse<Object>> DeleteMessageAsync(String[] occupantIds)
        {
            if (occupantIds == null)
                throw new ArgumentNullException("occupantIds");

            var uriMethod = String.Format(QuickbloxMethods.DeleteMessageMethod, String.Join(",", occupantIds));
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.DeleteAsync<Object>(this.quickbloxClient.ApiEndPoint, uriMethod, headers);
        }

        #endregion


    }
}
