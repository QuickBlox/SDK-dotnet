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

        public const string GetDialogsMethod = "/chat/Dialog.json";

        public const string UpdateDialogMethod = "/chat/Dialog/{0}.json";

        public const string DeleteDialogMethod = "/chat/Dialog/{0}.json";
        
        public const string GetMessagesMethod = "/chat/Message.json?chat_dialog_id={0}";

        public const string CreateMessageMethod = "/chat/Message.json";

        public const string UpdateMessageMethod = "/chat/Message/{0}.json";

        public const string DeleteMessageMethod = "/chat/Message/{0}.json";

        #endregion

        #region Messages

        public const string PushTokenMethod = "/push_tokens.json";

        public const string DeletePushTokenMethod = "/push_tokens/{0}.json";

        public const string SubscriptionsMethod = "/subscriptions.json";

        public const string DeleteSubscriptionsMethod = "/subscriptions/{0}.json";

        public const string EventsMethod = "/events.json";

        public const string GetEventByIdMethod = "/events/{0}.json";

        public const string EditEventMethod = "/events/{0}.json";

        public const string DeleteEventMethod = "/events/{0}.json";

        #endregion

        #region Users

        public const string UsersMethod = "/users.json";

        public const string GetUserMethod = "/users/{0}.json";

        public const string GetUserByLoginMethod = "/users/by_login.json";

        public const string GetUserByEmailMethod = "/users/by_email.json";

        public const string GetUserByTagsMethod = "/users/by_tags.json";
        
        public const string GetUserByFullMethod = "/users/by_full_name.json";

        public const string GetUserByFacebookIdMethod = "/users/by_facebook_id.json";

        public const string GetUserByTwitterIdMethod = "/users/by_twitter_id.json";

        public const string UpdateUserMethod = "/users/{0}.json";

        public const string DeleteUserMethod = "/users/{0}.json";

        public const string GetUserByExternalUserMethod = "/users/external/{0}.json";

        public const string DeleteUserByExternalUserMethod = "/users/external/{0}.json";

        public const string UserPasswordResetMethod = "/users/password/reset.json";

        #endregion

    }
}
