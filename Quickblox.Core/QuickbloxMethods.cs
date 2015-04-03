namespace Quickblox.Sdk.Core
{
    public static class QuickbloxMethods
    {
        public const string AccountMethod = "/account_settings.json";

        #region Auth

        public const string SessionMethod = "/session.json";

        public const string LoginMethod = "/login.json";

        #endregion

        #region Chat

        public const string CreateDialogMethod = "/chat/Dialog.json";

        public const string MessageMethod = "/chat/Message.json";

        #endregion

        #region Messages

        public const string PushTokenMethod = "/push_tokens.json";

        public const string DeletePushTokenMethod = "/push_tokens/{0}.json";

        public const string SubscriptionsMethod = "/subscriptions.json";

        public const string DeleteSubscriptionsMethod = "/subscriptions/{0}.json";

        public const string EventMethod = "/events.json";

        public const string DeleteEventMethod = "/events/{0}.json";

        #endregion

        #region Users

        public const string UsersMethod = "/users.json";

        #endregion

    }
}
