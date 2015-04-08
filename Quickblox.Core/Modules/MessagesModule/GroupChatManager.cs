using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class GroupChatManager
    {
        public event EventHandler OnMessageReceived;

        public event EventHandler OnLogin;

        public event EventHandler OnPresense;

        public GroupChatManager(string userId, string password, string groupId, string appId, string chatEndpoint)
        {
            
        }

        public void SendMessage(string message)
        {

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
