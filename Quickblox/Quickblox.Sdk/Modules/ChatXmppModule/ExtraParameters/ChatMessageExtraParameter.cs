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
                    new XElement("save_to_history", IsSaveToHistory ? "1" : "0"),
                    new XElement("dialog_id", DialogId)
                )
            );

            if (Attachment != null)
            {
                var attachmentElement = new XElement("attachment");
                attachmentElement.SetAttributeValue(XName.Get("id"), Attachment.Id);
                attachmentElement.SetAttributeValue(XName.Get("type"), Attachment.Type.ToString());
                var extraParams = srcTree.Descendants(XName.Get("extraParams")).First();
                extraParams.Add(attachmentElement);
            }

            return srcTree.Root;
        }
    }
}
