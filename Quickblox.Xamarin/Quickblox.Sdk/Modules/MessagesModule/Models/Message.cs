using System;

namespace Quickblox.Sdk.Modules.MessagesModule.Models
{
    public class Message
    {
        public string Id { get; set; }
        public Jid From { get; set; }
        public Jid To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Thread { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType MessageType { get; set; }

        public string XmlMessage { get; set; }
    }
}
