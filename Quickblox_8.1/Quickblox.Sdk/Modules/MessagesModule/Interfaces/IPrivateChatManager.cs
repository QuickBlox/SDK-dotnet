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
        event EventHandler<Message> OnMessageReceived;

        bool SendMessage(string message, Attachment attachment = null);
        Task<bool> AddToFriends(string friendName);
        Task<bool> AcceptFriend();
        Task<bool> RejectFriend();
        Task<bool> SendNotificationMessage(string notificationDialogId);

        void SubsribeForPresence();
        void ApproveSubscribtionRequest();
        void RejectSubscribtionRequest();
        void Unsubscribe();

        Task Block();
        Task Unblock();
    }
}
