using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.UsersModule.Requests
{
    public class UpdateUserRequest : BaseRequestSettings
    {
        [JsonProperty("user")]
        public UserRequest User { get; set; }
    }

    public class UpdateUserRequest<T> : BaseRequestSettings where T : UserRequest
    {
        [JsonProperty("user")]
        public T User { get; set; }
    }
}
