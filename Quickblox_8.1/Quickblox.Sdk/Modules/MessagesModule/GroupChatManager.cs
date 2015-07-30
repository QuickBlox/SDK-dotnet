using System;
using System.Net.Http.Headers;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using XMPP.tags.jabber.client;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    //TODO: use conditions if something was different
    #if Xamarin
    #endif

    public class GroupChatManager : IGroupChatManager
    {
        #region Fields

        private XMPP.Client xmppClient;
        private readonly string groupJid;

        #endregion

        #region Ctor

        public GroupChatManager(XMPP.Client client, string groupJid)
        {
            xmppClient = client;
            this.groupJid = groupJid;
        }

        #endregion

        #region IGroupChatManager members

        public void SendMessage(string message)
        {
            var msg = new message
            {
                to = groupJid,
                type = XMPP.tags.jabber.client.message.typeEnum.groupchat
            };

            var body = new body { Value = message };

            var extraParams = new ExtraParams();
            extraParams.Add(new SaveToHistory { Value = "1" });

            msg.Add(body, extraParams);

            xmppClient.Send(msg);
        }

        public void JoinGroup(string nickName)
        {
            string fullJid = string.Format("{0}/{1}", groupJid, nickName);

            var presense = new presence {to = fullJid};
            X x = new X();
            x.Add(new History() {Maxstanzas = "0"});
            presense.Add(x);

            xmppClient.Send(presense);
        }

        //public void RequestVoice()
        //{
        //    var msg = new message
        //    {
        //        to = groupJid
        //    };

        //    x x = new x { type = x.typeEnum.submit };

        //    var formTypeField = new field
        //    {
        //        var = "FORM_TYPE"
        //    };
        //    formTypeField.Add(new value { Value = "http://jabber.org/protocol/muc#request" });

        //    var mucRoleField = new field
        //    {
        //        type = field.typeEnum.text_single,
        //        label = "Requested role",
        //        var = "muc#role"
        //    };
        //    mucRoleField.Add(new value { Value = "participant" });

        //    x.Add(formTypeField, mucRoleField);
        //    msg.Add(x);

        //    xmppClient.Send(msg);
        //}

        #endregion

    }
}
