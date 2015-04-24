using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class GroupChatManager
    {
        private readonly XmppClientConnection xmpp;
        private readonly string groupJid;

        public GroupChatManager(XmppClientConnection xmppConnection, string groupJid)
        {
            xmpp = xmppConnection;
            this.groupJid = groupJid;
        }

        public bool SendMessage(string message)
        {
            if (!xmpp.Authenticated)
                return false;

            xmpp.Send(new agsXMPP.protocol.client.Message(groupJid, MessageType.groupchat, message));
            return true;
        }

        public bool JoinGroup(string nickName)
        {
            if (!xmpp.Authenticated)
                return false;

            string fullJid = string.Format("{0}/{1}", groupJid, nickName);

            xmpp.Send(new agsXMPP.protocol.client.Presence() {To = new Jid(fullJid)});
            return true;
        }

        public void Close()
        {

        }

        public void TurnOnAutoPresense()
        {

        }

        public void TurnOffAutoPresense()
        {
            
        }

    }
}
