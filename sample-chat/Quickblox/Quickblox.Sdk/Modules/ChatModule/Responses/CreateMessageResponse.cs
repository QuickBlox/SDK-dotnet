using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Models;

namespace Quickblox.Sdk.Modules.ChatModule.Responses
{
    public class CreateMessageResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }

        [JsonProperty("chat_dialog_id")]
        public string ChatDialogId { get; set; }

        [JsonProperty("date_sent")]
        public string DateSent { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("recipient_id")]
        public int RecipientId { get; set; }

        [JsonProperty("sender_id")]
        public int SenderId { get; set; }
    }
}
