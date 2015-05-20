using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class Tags
    {
        /// <summary>
        /// Should contain a string of tags divided by commas. Recipients (users) must have at least one tag that specified in list.
        /// </summary>
        [JsonProperty(PropertyName = "any", NullValueHandling = NullValueHandling.Ignore)]
        public TagsAny? TagsAny { get; set; }

        /// <summary>
        /// Should contain a string of tags divided by commas. Recipients (users) must exactly have only all tags that specified in list
        /// </summary>
        [JsonProperty(PropertyName = "all", NullValueHandling = NullValueHandling.Ignore)]
        public String TagsAll { get; set; }

        /// <summary>
        /// Should contain a string of tags divided by commas. Recipients (users) mustn't have tags that specified in list
        /// </summary>
        [JsonProperty(PropertyName = "exclude", NullValueHandling = NullValueHandling.Ignore)]
        public String TagsExclude { get; set; }
    }
}
