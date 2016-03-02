using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.Xmpp.Client;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public class GroupChatManager
    {
        private string dialogId;
        private string groupJid;
        private IQuickbloxClient quickbloxClient;
        private XmppClient xmppClient;

        public GroupChatManager(IQuickbloxClient quickbloxClient, XmppClient xmppClient, string groupJid, string dialogId)
        {
            this.quickbloxClient = quickbloxClient;
            this.xmppClient = xmppClient;
            this.groupJid = groupJid;
            this.dialogId = dialogId;
        }
    }
}
