using System;
using System.Collections.Generic;
using System.Text;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.ChatModule.Models;

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

        public static explicit operator MessageVm(Message message)
        {
            return new MessageVm()
            {
                MessageText = message.MessageText,
                DateTime = message.DateSent.ToDateTime()
            };
        }
    }
}
