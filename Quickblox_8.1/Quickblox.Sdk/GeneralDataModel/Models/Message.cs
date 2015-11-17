using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    /// <summary>
    /// Message class representing message from REST API and message from XMPP.
    /// </summary>
    public class Message
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }

        [JsonProperty("chat_dialog_id")]
        public string ChatDialogId { get; set; }

        [JsonProperty("date_sent")]
        public long DateSent { get; set; }

        [JsonProperty("message")]
        public string MessageText { get; set; }

        [JsonProperty("recipient_id")]
        public int? RecipientId { get; set; }

        [JsonProperty("sender_id")]
        public int SenderId { get; set; }

        [JsonProperty("read")]
        public int Read { get; set; }

        #region Notification properties

        [JsonProperty("notification_type")]
        public NotificationTypes NotificationType { get; set; }

        [JsonProperty("room_photo")]
        public string RoomPhoto { get; set; }

        [JsonProperty("room_name")]
        public string RoomName { get; set; }

        [JsonProperty("occupants_ids")]
        public string OccupantsIds { get; set; }

        #endregion

        #region XXMP chat properties

        public string From { get; set; }

        public string To { get; set; }

        public bool IsTyping { get; set; }

        public bool IsPausedTyping { get; set; }

        #endregion
    }
}
