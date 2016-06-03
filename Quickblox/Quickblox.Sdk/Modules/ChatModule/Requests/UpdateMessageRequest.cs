using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.ChatModule.Requests
{
    public class UpdateMessageRequest
    {
        [JsonProperty("read")]
        public string Read { get; set; }

        [JsonProperty("chat_dialog_id")]
        public string ChatDialogId { get; set; }
    }
}
