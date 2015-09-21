using System;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public string MessageText { get; set; }
        public Attachment[] Attachments { get; set; }
        public DateTime DateTimeSent { get; set; }
        public string DialogId { get; set; }
        public bool IsTyping { get; set; }
        public bool IsPausedTyping { get; set; }
    }
}
