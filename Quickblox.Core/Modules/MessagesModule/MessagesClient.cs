using System;
using System.Diagnostics;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        private QuickbloxClient quickbloxClient;

        public MessagesClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        public int UserId { get; set; }
        public string Password { get; set; }
        public int AppId { get; set; }
        public string ChatEndpoint { get; set; }

        public PrivateChatManager InitPrivateChatManager(int otherUserId)
        {
            if(UserId == 0)
                Debug.WriteLine("User ID is 0 when creating private chat manager.");

            if (Password == null)
                Debug.WriteLine("Password is null when creating private chat manager.");

            if (AppId == 0)
                Debug.WriteLine("App ID is 0 when creating private chat manager.");

            if(ChatEndpoint == null)
                throw new Exception("ChatEndpoint must be not null");

            return new PrivateChatManager(UserId, Password, otherUserId, AppId, ChatEndpoint);
        }

        public GroupChatManager InitGroupChatManager(string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
