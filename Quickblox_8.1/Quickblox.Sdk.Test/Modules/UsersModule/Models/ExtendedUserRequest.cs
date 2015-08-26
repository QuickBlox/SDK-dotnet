using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quickblox.Sdk.Modules.UsersModule.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Test.Modules.UsersModule.Models
{
    public class ExtendedUserRequest : UserRequest
    {
        [JsonProperty(PropertyName = "sex")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; internal set; }

        [JsonProperty(PropertyName = "show_me")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShowMe ShowMe { get; internal set; }

        [JsonProperty(PropertyName = "about_me")]
        public String AboutMe { get; set; }
    }

    public enum ShowMe
    {
        men = 1,
        woman = 2,
        all = 3
    }

    public enum Gender
    {
        male = 1,
        female = 2
    }
}
