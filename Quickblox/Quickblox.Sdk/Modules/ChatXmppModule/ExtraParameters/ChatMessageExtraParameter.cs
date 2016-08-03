using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Xml.Linq;
using System.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class ChatMessageExtraParameter : IExtraParameter
    {
        public string DialogId { get; private set; }

        public bool IsSaveToHistory { get; private set; }

        public AttachmentExtraParamValue Attachment { get; private set; }
        
        public ChatMessageExtraParameter(string dialogId, bool isSaveToHistory, AttachmentExtraParamValue attachment = null)
        {
            DialogId = dialogId;
            IsSaveToHistory = isSaveToHistory;
            Attachment = attachment;
        }

        public XElement Build()
        {
            XDocument srcTree = new XDocument(
                new XElement(XName.Get("extraParams", "jabber:client"),
				             new XElement(XName.Get("save_to_history", "jabber:client"), IsSaveToHistory ? "1" : "0"),
				             new XElement(XName.Get("dialog_id", "jabber:client"), DialogId)
                )
            );

            if (Attachment != null)
            {
				var attachmentElement = new XElement(XName.Get("attachment", "jabber:client"));
                attachmentElement.SetAttributeValue(XName.Get("id", "jabber:client"), Attachment.Id);
                attachmentElement.SetAttributeValue(XName.Get("type", "jabber:client"), Attachment.Type.ToString());
                var extraParams = srcTree.Descendants(XName.Get("extraParams", "jabber:client")).First();
                extraParams.Add(attachmentElement);
            }

            return srcTree.Root;
        }
    }
}
