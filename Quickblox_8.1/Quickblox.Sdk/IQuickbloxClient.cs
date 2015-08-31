using System;
using System.Threading.Tasks;
using Quickblox.Sdk.Cryptographic;
using Quickblox.Sdk.Modules.AuthModule;
using Quickblox.Sdk.Modules.ChatModule;
using Quickblox.Sdk.Modules.ContentModule;
using Quickblox.Sdk.Modules.CustomObjectModule;
using Quickblox.Sdk.Modules.LocationModule;
using Quickblox.Sdk.Modules.MessagesModule;
using Quickblox.Sdk.Modules.MessagesModule.Interfaces;
using Quickblox.Sdk.Modules.NotificationModule;
using Quickblox.Sdk.Modules.UsersModule;

namespace Quickblox.Sdk
{
    public interface IQuickbloxClient
    {
        ContentClient ContentClient { get; }

        AuthorizationClient CoreClient { get; }

        ChatClient ChatClient { get; }

        UsersClient UsersClient { get; }

        LocationClient LocationClient { get; }

        NotificationClient NotificationClient { get; }

        IMessagesClient MessagesClient { get; }

        CustomObjectsClient CustomObjectsClient { get; }

        DateTime LastRequest { get; }

        string ApiEndPoint { get; }

        string ChatEndpoint { get; }

        string Token { get; set; }

        Task GetAccountSettingsAsync(string accountKey);
    }
}
