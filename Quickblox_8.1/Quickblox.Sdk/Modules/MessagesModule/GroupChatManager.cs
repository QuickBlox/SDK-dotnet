using agsXMPP;
using agsXMPP.protocol.client;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using AgsMessage = agsXMPP.protocol.client.Message;
using AgsPresence = agsXMPP.protocol.client.Presence;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    public class GroupChatManager : IGroupChatManager
    {
        #region Fields

        private readonly XmppClientConnection xmpp;
        private readonly string groupJid;

        #endregion

        #region Ctor

        public GroupChatManager(XmppClientConnection xmppConnection, string groupJid)
        {
            xmpp = xmppConnection;
            this.groupJid = groupJid;
        }

        #endregion

        #region IGroupChatManager members

        public void SendMessage(string message)
        {
            xmpp.Send(new AgsMessage(groupJid, MessageType.groupchat, message));
        }

        public void JoinGroup(string nickName)
        {
            string fullJid = string.Format("{0}/{1}", groupJid, nickName);

            xmpp.Send(new AgsPresence {To = new Jid(fullJid)});
        }

        #endregion


    }
}
