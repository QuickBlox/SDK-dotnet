using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.ChatModule.Models;

namespace Quickblox.Sdk.Modules.ChatModule.Responses
{
    public class RetrieveMessagesResponse : ListResponse<Message> { }

    public class RetrieveMessagesResponse<T> : ListResponse<T> where T : Message { }
}
