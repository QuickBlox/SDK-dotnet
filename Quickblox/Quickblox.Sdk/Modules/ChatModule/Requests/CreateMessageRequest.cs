using Newtonsoft.Json;
using Quickblox.Sdk.GeneralDataModel.Models;
using Quickblox.Sdk.GeneralDataModel.Request;

namespace Quickblox.Sdk.Modules.ChatModule.Requests
{
    public class CreateMessageRequest : BaseRequestSettings
    {
        [JsonProperty("chat_dialog_id")]
        public string ChatDialogId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("recipient_id")]
        public int RecipientId { get; set; }

        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }

        [JsonProperty("send_to_chat")]
        public int SendToChat { get; set; } // 0 or 1

        [JsonProperty("markable")]
        public int Markable { get; set; } // 0 or 1
    }
}
