using System;

namespace Quickblox.Sdk.Modules.MessagesModule
{
    public class MessagesClient
    {
        private QuickbloxClient quickbloxClient;

        public MessagesClient(QuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string AppId { get; set; }
        public string ChatEndpoint { get; set; }

        public PrivateChatManager InitPrivateChatManager(string otherUserId)
        {
            if(Login == null || Password == null || AppId == null || ChatEndpoint == null)
                throw new Exception("Login, password, appId, Chatendpoints must be not null");

            return new PrivateChatManager(Login, Password, otherUserId, AppId, ChatEndpoint);
        }

        public GroupChatManager InitGroupChatManager(string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
