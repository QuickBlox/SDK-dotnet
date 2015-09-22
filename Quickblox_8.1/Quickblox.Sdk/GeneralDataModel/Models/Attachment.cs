using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    [DataContract(Name = "attachment")]
    public class Attachment
    {
        [XmlAttribute("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlAttribute("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}
