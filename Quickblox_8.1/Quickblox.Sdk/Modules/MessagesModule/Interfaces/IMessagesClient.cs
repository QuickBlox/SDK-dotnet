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
        event EventHandler OnContactsChanged;

        int ApplicationId { get; }
        string ChatEndpoint { get; }
        List<Contact> Contacts { get; }
        List<Presence> Presences { get; }
        bool IsConnected { get; }

        Task Connect(string chatEndpoint, int userId, int applicationId, string password);
        void Disconnect();
        IPrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId = null);
        IGroupChatManager GetGroupChatManager(string groupJid, string dialogId);
        void ReloadContacts();
        void AddContact(Contact contact);
        void DeleteContact(int userId);
    }
}