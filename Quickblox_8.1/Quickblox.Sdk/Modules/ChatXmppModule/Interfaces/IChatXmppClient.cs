using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Interfaces
{
    /// <summary>
    /// XMPP chat client interface.
    /// </summary>
    public interface IChatXmppClient
    {
        /// <summary>
        /// Event occuring when a new message is received.
        /// </summary>
        event EventHandler<Message> MessageReceived;

        /// <summary>
        /// Event occuring when a new System message is received.
        /// </summary>
        event EventHandler<SystemMessage> SystemMessageReceived;

        /// <summary>
        /// Event occuring  when a presence is received.
        /// </summary>
        event EventHandler<Presence> PresenceReceived;
        
        /// <summary>
        /// Event occuring  when your contacts in roster have changed.
        /// </summary>
        event EventHandler ContactsChanged;

        /// <summary>
        /// Event occuring when a contact is added to contact list.
        /// </summary>
        event EventHandler<Contact> ContactAdded;

        /// <summary>
        /// Event occuring when a contact is removed from contact list.
        /// </summary>
        event EventHandler<Contact> ContactRemoved;

        /// <summary>
        /// Event occuring when xmpp connection is lost.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        /// Quickblox Application ID.
        /// </summary>
        int ApplicationId { get; }

        /// <summary>
        /// XMPP chat endpoint.
        /// </summary>
        string ChatEndpoint { get; }

        /// <summary>
        /// Contacts list in roster.
        /// </summary>
        List<Contact> Contacts { get; }

        /// <summary>
        /// Presences list.
        /// </summary>
        List<Presence> Presences { get; }

        /// <summary>
        /// Is XMPP connection open
        /// </summary>
        bool IsConnected { get; }

#if DEBUG || TEST_RELEASE
        string DebugClientName { get; set; }
#endif

        /// <summary>
        /// Connects to XMPP server.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="password">User password</param>
        /// <returns>Async operation result</returns>
        Task Connect(int userId, string password);

        /// <summary>
        /// Disconnects from XMPP server.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Creates a private one-to-one chat manager.
        /// </summary>
        /// <param name="otherUserId">Another user ID</param>
        /// <param name="dialogId">Dialog ID with another user</param>
        /// <returns>PrivateChatManager instance.</returns>
        IPrivateChatManager GetPrivateChatManager(int otherUserId, string dialogId);

        /// <summary>
        /// Creates a group chat manager.
        /// </summary>
        /// <param name="groupJid">Group XMPP room JID.</param>
        /// <param name="dialogId">Group dialog ID.</param>
        /// <returns>GroupChatManager</returns>
        IGroupChatManager GetGroupChatManager(string groupJid, string dialogId);

        /// <summary>
        /// Requests roster contact list from server.
        /// </summary>
        void ReloadContacts();

        /// <summary>
        /// Enables Message Carbons which allows to have sync conversations in case a user has several devices.
        /// </summary>
        void EnableMessageCarbons();
    }

    internal interface IRosterManager
    {
        void ReloadContacts();
        void AddContact(Contact contact);
        void DeleteContact(int userId);
    }
}