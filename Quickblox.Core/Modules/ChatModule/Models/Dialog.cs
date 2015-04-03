using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.Models
{
    public class Dialog
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("last_message")]
        public string LastMessage { get; set; }

        [JsonProperty("last_message_date_sent")]
        public string LastMessageDateSent { get; set; }

        [JsonProperty("last_message_user_id")]
        public int LastMessageUserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("occupants_ids")]
        public int[] OccupantsIds { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("unread_messages_count")]
        public int UnreadMessagesCount { get; set; }

        [JsonProperty("xmpp_room_jid")]
        public string XmppRoomJid { get; set; }
    }
}
