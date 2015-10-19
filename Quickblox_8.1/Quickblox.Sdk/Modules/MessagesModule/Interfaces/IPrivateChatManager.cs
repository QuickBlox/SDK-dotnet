using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IPrivateChatManager
    {
        event EventHandler OnIsTyping;
        event EventHandler OnPausedTyping;
        event EventHandler<Message> OnMessageReceived;

        bool SendMessage(string message);
        bool AddToFriends(string friendName = null);
        bool AcceptFriend(string friendName = null);
        bool RejectFriend();
        bool DeleteFromFriends();
        bool NotifyAboutGroupCreation(string createdDialogId);

        void NotifyIsTyping();
        void NotifyPausedTyping();
    }
}
