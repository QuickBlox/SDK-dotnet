using Quickblox.Sdk.GeneralDataModel.Models;
using System;
using System.Collections.Generic;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IGroupChatManager
    {
        event EventHandler<Message> OnMessageReceived;

        void JoinGroup(string nickName);
        bool SendMessage(string message);

        bool NotifyAboutGroupCreation(IList<int> occupantsIds);
        bool NotifyGroupImageChanged(string groupImageUrl);
        bool NotifyGroupNameChanged(string groupName);
    }
}
