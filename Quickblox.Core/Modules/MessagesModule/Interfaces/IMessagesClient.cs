using System;
using System.Collections.Generic;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    public interface IMessagesClient
    {
        event EventHandler OnConnected;
        event EventHandler<Message> OnMessageReceived;
        event EventHandler<Presence> OnPresenceReceived;
        event EventHandler OnContactsLoaded;
        List<Contact> Contacts { get; }
        bool IsConnected { get; }
        void Connect(int userId, string password, int applicationId, string chatEndpoint);
        IPrivateChatManager GetPrivateChatManager(int otherUserId);
        IGroupChatManager GetGroupChatManager(string groupJid);
        void ReloadContacts();
        void AddContact(Contact contact);
        void DeleteContact(int userId);
    }
}