using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.ChatModule.Models;

namespace Quickblox.Sdk.Modules.Models
{
    public class Dialog
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdateAt { get; set; }

        [JsonProperty("last_message")]
        public string LastMessage { get; set; }

        [JsonProperty("last_message_date_sent")]
        public long? LastMessageDateSent { get; set; }

        [JsonProperty("last_message_user_id")]
        public int? LastMessageUserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("occupants_ids")]
        public IList<int> OccupantsIds { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("type")]
        public DialogType Type { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("xmpp_room_jid")]
        public string XmppRoomJid { get; set; }

        [JsonProperty("unread_messages_count")]
        public int? UnreadMessagesCount { get; set; }
    }
}
