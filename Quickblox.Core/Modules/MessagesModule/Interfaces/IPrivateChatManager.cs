using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IPrivateChatManager
    {
        void SendMessage(string message, Attachment attachment = null);
        void SubsribeForPresence();
        void ApproveSubscribtionRequest();
        void DeclineSubscribtionRequest();
        void Unsubscribe();
        void SendPresenceInformation(PresenceType presenceType);
    }
}
