using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quickblox.Sdk.Converters;
using System.Xml.Linq;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    /// <summary>
    /// XmppMessage class representing XmppMessage from REST API and XmppMessage from XMPP.
    /// </summary>
    public class Message
    {
        public Message()
        {
            OccupantsIds = new List<int>();
            CurrentOccupantsIds = new List<int>();
            AddedOccupantsIds = new List<int>();
            DeletedOccupantsIds = new List<int>();
        }

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
        [JsonConverter(typeof(StringIntListConverter))]
        public IList<int> OccupantsIds { get; set; }

        [JsonProperty("current_occupant_ids")]
        [JsonConverter(typeof(StringIntListConverter))]
        public IList<int> CurrentOccupantsIds { get; set; }

        [JsonProperty("added_occupant_ids")]
        [JsonConverter(typeof(StringIntListConverter))]
        public IList<int> AddedOccupantsIds { get; set; }

        [JsonProperty("deleted_occupant_ids")]
        [JsonConverter(typeof(StringIntListConverter))]
        public IList<int> DeletedOccupantsIds { get; set; }

        [JsonProperty("deleted_id")]
        public int DeletedId { get; set; }

        [JsonProperty("room_updated_date")]
        public double RoomUpdateDate { get; set; }

        #endregion

        #region XXMP chat properties

        public string From { get; set; }

        public string To { get; set; }

        public XElement ExtraParameters { get; internal set; }

#if !Xamarin
        public bool IsTyping { get; set; }

        public bool IsPausedTyping { get; set; }
#endif

        #endregion
    }
}
