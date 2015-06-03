using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IMessagesClient
    {
        event EventHandler<Message> OnMessageReceived;
        event EventHandler<Presence> OnPresenceReceived;
        event EventHandler OnContactsLoaded;

        List<Contact> Contacts { get; }
        bool IsConnected { get; }

        Task Connect(string chatEndpoint, int userId, int applicationId, string password);
        IPrivateChatManager GetPrivateChatManager(int otherUserId);
        IGroupChatManager GetGroupChatManager(string groupJid);
        void ReloadContacts();
        void AddContact(Contact contact);
        void DeleteContact(int userId);
    }
}