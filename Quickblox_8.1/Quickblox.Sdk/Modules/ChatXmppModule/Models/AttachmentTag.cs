using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public class AttachmentTag : Tag
    {
        public string Id
        {
            get { return (string)GetAttributeValue("id"); }
            set { SetAttributeValue("id", value); }
        }

        public string Type
        {
            get { return (string)GetAttributeValue("type"); }
            set { SetAttributeValue("type", value); }
        }

        public string Name
        {
            get { return (string)GetAttributeValue("name"); }
            set { SetAttributeValue("name", value); }
        }

        public string Url
        {
            get { return (string)GetAttributeValue("url"); }
            set { SetAttributeValue("url", value); }
        }

        #region Ctor

        public AttachmentTag() : base(ExtraParams.GetXNameFor(ExtraParamsList.attachment))
        {
        }

        public AttachmentTag(XElement other) : base(other)
        {
        }

        #endregion
    }
}
