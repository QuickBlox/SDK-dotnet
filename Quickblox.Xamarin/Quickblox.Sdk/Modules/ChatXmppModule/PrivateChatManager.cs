using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using Sharp.Xmpp.Client;
using System.Threading.Tasks;
using System.Net;
using Quickblox.Sdk.Modules.ChatModule.Models;
using System;
using System.Xml.Linq;

namespace Quickblox.Sdk
{
    /// <summary>
    /// Private chat manager. Creates manager for personal chat
    /// </summary>
    internal class PrivateChatManager : IPrivateChatManager
    {
        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;

        private readonly Jid otherUserJid;

        private readonly int otherUserId;
        private string dialogId;

        //public event EventHandler<Message> OnMessageReceived;

        internal PrivateChatManager(IQuickbloxClient quickbloxClient, XmppClient xmppClient, int otherUserId, string dialogId)
        {
            this.otherUserId = otherUserId;
            this.otherUserJid = quickbloxClient.ChatXmppClient.BuildJid(otherUserId);
            this.dialogId = dialogId;
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
        }

        public void SendMessage(string body, string subject = null, string thread = null, MessageType messageType = MessageType.Chat, NotificationType notificationType = NotificationType.None, bool saveToHistory = true)
        {
            var wrappedMessageType = (Sharp.Xmpp.Im.MessageType)Enum.Parse(typeof(Sharp.Xmpp.Im.MessageType), messageType.ToString());
            var jid = new Sharp.Xmpp.Jid(otherUserJid.ToString());
            string extraParams = "";
            if (saveToHistory)
                extraParams = SaveToHistory(dialogId);
            xmppClient.SendMessage(jid, body, extraParams, subject, thread, wrappedMessageType);
        }

        private string SaveToHistory(string dialogId)
        {
            XDocument srcTree = new XDocument(
                 new XElement("extraParams",
                     new XElement("save_to_history", "1"),
                     new XElement("dialog_id", dialogId)
                 )
             );

            return srcTree.ToString();
        }
    }
}


