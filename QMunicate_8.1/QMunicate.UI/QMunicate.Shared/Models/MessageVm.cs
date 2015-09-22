using System;
using System.Collections.Generic;
using System.Text;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.MessagesModule.Models;
using Message = Quickblox.Sdk.GeneralDataModel.Models.Message;

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
        #region Ctor

        public MessageVm() { }

        protected MessageVm(Message message, int curentUserId = default(int))
        {
            MessageText = message.MessageText;
            DateTime = message.DateSent.ToDateTime();
            if (curentUserId != default(int))
                MessageType = message.SenderId == curentUserId ? MessageType.Outgoing : MessageType.Incoming;

            if (message.NotificationType.HasValue && Enum.IsDefined(typeof (NotificationTypes), message.NotificationType))
                NotificationType = (NotificationTypes) message.NotificationType;
        }

        #endregion

        #region Properties

        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationTypes NotificationType { get; set; }

        #endregion

        #region Public methods

        public static MessageVm FromMessage(Message message, int curentUserId = default(int))
        {
            return new MessageVm(message, curentUserId);
        }

        #endregion


    }
}
