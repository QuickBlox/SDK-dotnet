using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    [DataContract(Name = "attachment")]
    public class Attachment
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
