using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IGroupChatManager
    {
        void JoinGroup(string nickName);
        void SendMessage(string message);
    }
}
