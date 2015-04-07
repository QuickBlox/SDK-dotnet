using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.UsersModule.Requests
{
    public class UserSignUpRequest : BaseRequestSettings
    {
        [JsonProperty("user")]
        public UserRequest User { get; set; }
    }
}
