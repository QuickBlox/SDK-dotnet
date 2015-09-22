using Newtonsoft.Json;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
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

        [JsonProperty("notification_type")]
        public int? NotificationType { get; set; }

        [JsonProperty("occupants_ids")]
        public string OccupantsIds { get; set; }
    }
}
