using System.Xml.Linq;
using Newtonsoft.Json;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    public class Attachment : Tag
    {
        [JsonProperty("id")]
        public string Id
        {
            get { return (string) GetAttributeValue("id"); }
            set { SetAttributeValue("id", value); }
        }

        [JsonProperty("type")]
        public string Type
        {
            get { return (string) GetAttributeValue("type"); }
            set { SetAttributeValue("type", value); }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return (string) GetAttributeValue("name"); }
            set { SetAttributeValue("name", value); }
        }

        [JsonProperty("url")]
        public string Url
        {
            get { return (string) GetAttributeValue("url"); }
            set { SetAttributeValue("url", value); }
        }

        #region Ctor

        public Attachment() : base(XName.Get("attachment", "jabber:client"))
        {
        }

        public Attachment(XElement other) : base(other)
        {
        }

        #endregion
    }
}