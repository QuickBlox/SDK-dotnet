using System;
using System.Collections.Generic;
using System.Text;

namespace QMunicate.Models
{
    public enum MessageType
    {
        Unknown,
        Incoming,
        Outgoing
    }

    public class MessageVm
    {
        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
    }
}
