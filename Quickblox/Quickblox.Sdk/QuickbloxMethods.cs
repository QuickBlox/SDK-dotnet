using System;

namespace Quickblox.Sdk.Core
{
    internal static class QuickbloxMethods
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

        public const string GetMessagesMethod = "/chat/XmppMessage.json";

        public const string CreateMessageMethod = "/chat/XmppMessage.json";

        public const string UpdateMessageMethod = "/chat/XmppMessage/{0}.json";

        public const string DeleteMessageMethod = "/chat/XmppMessage/{0}.json";

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

        #region Content

        public const string CreateFileMethod = "/blobs.json";

        public const string GetFilesMethod = "/blobs.json";

        public const string GetTaggedFilesMethod = "/blobs/tagged.json";

        public const string UploadMethod = "{0}.json";

        public const string CompleteUploadByFileIdMethod = "/blobs/{0}/complete.json";

        public const string GetFileByIdMethod = "/blobs/{0}.json";

        public const string DownloadFileByUIdMethod = "/blobs/{0}";

        public const string DownloadFileByIdMethod = "/blobs/{0}/download.json";

        public const string GetFileByIdReadOnlyMethod = "/blobs/{0}/getblobobjectbyid";

        public const string EditFileMethod = "/blobs/{0}";

        public const string DeleteFileMethod = "/blobs/{0}";

        #endregion

        #region CustomObject
        
        public const string RetriveObjectsByIdsMethod = "/data/{0}/{1}.json";
        public const string RetriveObjectsMethod = "/data/{0}.json";
        public const string CreateCustomObjectMethod = "/data/{0}.json";
        public const string CreateMultiCustomObjectMethod = "/data/{0}/multi.json";
        public const string UpdateCustomObjectMethod = "/data/{0}/{1}.json";
        public const string UpdateMultiCustomObjectMethod = "/data/{0}/multi.json";
        public const string DeleteCustomObjectMethod = "/data/{0}/{1}.json";

        /// <summary>
        /// https://api.quickblox.com/data/<parent_class_name>/<parent_record_id>/<child_class_name>
        /// </summary>
        public const string CreateRelationMethod = "/data/{0}/{1}/{2}.json";
        public const string GetRelatedObjectsMethod = "/data/{0}/{1}/{2}.json";

        #endregion

        #region Location

        public const string CreateGeoDataMethod = "/geodata.json";
        public const string UpdateByIdGeoDataMethod = "/geodata/{0}.json";
        public const string GetByIdGeoDataMethod = "/geodata/{0}.json";
        public const string FindGeoDataMethod = "/geodata/find.json";
        public const string DeleteByIdGeoDataMethod = "/geodata/{0}.json";
        public const string DeleteGeoWithDaysDataMethod = "/geodata.json?days={0}";

        #endregion
    }
}
