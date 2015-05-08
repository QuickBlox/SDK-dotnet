using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    [DataContract(Name = "attachment")]
    public class Attachment
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}
