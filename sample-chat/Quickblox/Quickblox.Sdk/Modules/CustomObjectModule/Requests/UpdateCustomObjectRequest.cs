using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Requests
{
    public class UpdateCustomObjectRequest<T> : BaseRequestSettings where T: BaseCustomObject
    {
        public T CustomObject { get; set; }
    }
}
