using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.Modules.MessagesModule.Models;


namespace Quickblox.Sdk.Modules.MessagesModule.Interfaces
{
    /// <summary>
    /// XMPP chat client interface.
    /// </summary>
    public interface IMessagesClient
    {
        /// <summary>
        /// Event when a new message is received.
        /// </summary>
        event EventHandler<Message> OnMessageReceived;

        /// <summary>
        /// Event when a presence is received.
        /// </summary>
        event EventHandler<Presence> OnPresenceReceived;
        
        /// <summary>
        /// Event when your contacts in roster have changed.
        /// </summary>
        event EventHandler OnContactsChanged;

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
        /// Bool representing if
        /// </summary>
        bool IsConnected { get; }

#if DEBUG || TEST_RELEASE
        string DebugClientName { get; set; }
#endif

        /// <summary>
        /// Connects to XMPP server.
        /// </summary>
        /// <param name="chatEndpoint">XMPP chatendpoint</param>
        /// <param name="userId">User ID</param>
        /// <param name="applicationId">Quickblox application ID</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        Task Connect(string chatEndpoint, int userId, int applicationId, string password);

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
        /// <returns></returns>
        IGroupChatManager GetGroupChatManager(string groupJid, string dialogId);

        /// <summary>
        /// Requests roster contact list from server.
        /// </summary>
        void ReloadContacts();
    }

    internal interface IRosterManager
    {
        void ReloadContacts();
        void AddContact(Contact contact);
        void DeleteContact(int userId);
    }
}