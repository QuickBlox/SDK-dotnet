﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quickblox.Sdk.Modules.CustomObjectModule.Models
{
    public class Permissions
    {
        [JsonProperty(PropertyName = "read")]
        public ReadPermissions ReadPermissions { get; set; }

        [JsonProperty(PropertyName = "update")]
        public UpdatePermissions UpdatePermissions { get; set; }

        [JsonProperty(PropertyName = "delete")]
        public DeletePermissions DeletePermissions { get; set; }
    }

    public class ReadPermissions
    {
        [JsonProperty(PropertyName = "access")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Access AccessState { get; set; }
    }

    public class UpdatePermissions
    {
        [JsonProperty(PropertyName = "access")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Access AccessState { get; set; }
    }

    public class DeletePermissions
    {
        [JsonProperty(PropertyName = "access")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Access AccessState { get; set; }
    }

    public enum Access
    {
        open,
        owner
    }
}
