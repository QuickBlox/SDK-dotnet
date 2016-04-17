using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    /// <summary>
    /// A message that contains a forwarded message sent by the current user, but from a different device.
    /// </summary>
    public class MessageCarbonsMessageSent : XElement
    {
        public static XName XName = XName.Get("sent", "urn:xmpp:carbons:2");

        public MessageCarbonsMessageSent() : base(XName)
        {
        }
    }

    /// <summary>
    /// Element that contains the message sent by the current user, but from a different device.
    /// </summary>
    public class ForwardedMessage : XElement
    {
        public static XName XName = XName.Get("forwarded", "urn:xmpp:forward:0");

        public ForwardedMessage() : base(XName)
        {
        }
    }

    // Message example:
    //    <message from = "5719149-13318@chat.quickblox.com" id="56a795eb23cbdf790d002f53" to="5719149-13318@chat.quickblox.com/1220770403-quickblox-3567" type="chat" xmlns="jabber:client">
    //	<sent xmlns = "urn:xmpp:carbons:2" >
    //        < forwarded xmlns="urn:xmpp:forward:0">
    //			<message from = "5719149-13318@chat.quickblox.com/1220770403-quickblox-3557" id="56a795eb23cbdf790d002f53" to="5513419-13318@chat.quickblox.com" type="chat" xmlns="jabber:client">
    //				<body>Test message</body>
    //				<extraParams>
    //					<save_to_history>1</save_to_history>
    //					<dialog_id>560ba81da28f9ac8c2000657</dialog_id>
    //				</extraParams>
    //			</message>
    //		</forwarded>
    //	</sent>
    // </message>

}
