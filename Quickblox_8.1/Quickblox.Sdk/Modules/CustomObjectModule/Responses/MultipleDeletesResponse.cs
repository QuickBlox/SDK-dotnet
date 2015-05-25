using Newtonsoft.Json;
using Quickblox.Sdk.Modules.CustomObjectModule.Models;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Responses
{
    public class MultipleDeletesResponse
    {
        [JsonProperty(PropertyName = "SuccessfullyDeleted")]
        public SuccessfullyDeleted SuccessfullyDeletedItems { get; set; }

        [JsonProperty(PropertyName = "WrongPermissions")]
        public WrongPermissions WrongPermissionsItems { get; set; }

        [JsonProperty(PropertyName = "NotFound")]
        public NotFoundsItems NotFoundsItems { get; set; }
    }
}
