using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IGroupChatManager
    {
        event EventHandler<Message> OnMessageReceived;

        void JoinGroup(string nickName);
        bool SendMessage(string message);
    }
}
