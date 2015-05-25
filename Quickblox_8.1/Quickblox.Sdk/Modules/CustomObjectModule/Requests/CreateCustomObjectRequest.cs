using Quickblox.Sdk.GeneralDataModel.Request;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Requests
{
    public class CreateCustomObjectRequest<T> : BaseRequestSettings where T: BaseCustomObject
    {
        public T Type { get; set; }
    }
}
