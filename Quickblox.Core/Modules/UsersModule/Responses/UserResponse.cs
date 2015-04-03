using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.UsersModule.Responses
{
    public class UserResponse
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
