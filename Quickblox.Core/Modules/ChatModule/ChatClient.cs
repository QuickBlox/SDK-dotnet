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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialogName"></param>
        /// <param name="dialogType"></param>
        /// <param name="occupantsIds">Occupants IDs separated by comma</param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public async Task<HttpResponse<CreateDialogResponse>> CreateDialog(string dialogName, DialogType dialogType, string occupantsIds = null, string photoId = null)
        {
            var createDialogRequest = new CreateDialogRequest {Type = (int) dialogType, Name = dialogName, OccupantsIds = occupantsIds, Photo = photoId};
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateDialogResponse, CreateDialogRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.CreateDialogMethod, new NewtonsoftJsonSerializer(), createDialogRequest, headers);
        }

        public async Task<HttpResponse<CreateMessageResponse>> CreateMessage(CreateMessageRequest createMessageRequest)
        {
            var headers = RequestHeadersBuilder.GetDefaultHeaders().GetHeaderWithQbToken(this.quickbloxClient.Token);
            return await HttpService.PostAsync<CreateMessageResponse, CreateMessageRequest>(this.quickbloxClient.ApiEndPoint,
                        QuickbloxMethods.MessageMethod, new NewtonsoftJsonSerializer(), createMessageRequest, headers);
        }

        #endregion


    }
}
