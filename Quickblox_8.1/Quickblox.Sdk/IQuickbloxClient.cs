using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.AuthModule.Response;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.CustomObjectModule;
using Quickblox.Sdk.Modules.LocationModule;
using Quickblox.Sdk.Modules.MessagesModule;
#if !Xamarin
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
#endif
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;

namespace Quickblox.Sdk
{
    /// <summary>
    /// IQuickbloxClient interface.
    /// </summary>
    public interface IQuickbloxClient
    {
        /// <summary>
        /// Content module allows to manage app contents and settings.
        /// </summary>
        ContentClient ContentClient { get; }

        /// <summary>
        /// Authorization module allows to manage user sessions.
        /// </summary>
        AuthorizationClient CoreClient { get; }

        /// <summary>
        /// Chat module allows to manage user dialogs.
        /// </summary>
        ChatClient ChatClient { get; }

        /// <summary>
        /// User module manages all things related to user accounts handling, authentication, account data, password reminding etc.
        /// </summary>
        UsersClient UsersClient { get; }

        /// <summary>
        /// Notification module allows to manage push and email notifications to users.
        /// </summary>
        NotificationClient NotificationClient { get; }

        /// <summary>
        /// Location module allows to work with user locations.
        /// </summary>
        LocationClient LocationClient { get; }

        /// <summary>
        /// Messages module allows users to chat with each other in private or group dialogs via XMPP protocol.
        /// </summary>
#if !Xamarin
        IMessagesClient MessagesClient { get; }
#else
        MessagesClient MessagesClient { get; }
#endif

        /// <summary>
        /// Custom Objects module provides flexibility to define any data structure(schema) you need.
        /// Schema is defined in QuickBlox Administration Panel. The schema is called Class and contains field names and their type.
        /// </summary>
        CustomObjectsClient CustomObjectsClient { get; }

        /// <summary>
        /// API endpoint
        /// </summary>
        string ApiEndPoint { get; }

        /// <summary>
        /// XMPP Chat endpoint
        /// </summary>
        string ChatEndpoint { get; }

        /// <summary>
        /// Quickblox token. Must be set before calling any methods that require authentication.
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// UTC DateTime of the last request to the server.
        /// </summary>
        DateTime LastRequest { get; }

        /// <summary>
        /// Returns account settings (account ID, endpoints, etc.)
        /// </summary>
        /// <param name="accountKey">Account key from admin panel</param>
        /// <returns>AccountResponse</returns>
        Task<HttpResponse<AccountResponse>> GetAccountSettingsAsync(string accountKey);
    }
}
