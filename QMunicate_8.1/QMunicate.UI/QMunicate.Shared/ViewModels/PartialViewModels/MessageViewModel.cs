using System;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.GeneralDataModel.Models;
using Message = Quickblox.Sdk.GeneralDataModel.Models.Message;

namespace QMunicate.ViewModels.PartialViewModels
{
    public enum MessageType
    {
        Unknown,
        Incoming,
        Outgoing
    }

    public class MessageViewModel
    {
        #region Properties

        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationTypes NotificationType { get; set; }
        public int SenderId { get; set; }

        #endregion

        #region Public methods

        public static MessageViewModel FromMessage(Message message, int curentUserId = default(int))
        {
            var messageViewModel = new MessageViewModel
            {
                MessageText = message.MessageText,
                DateTime = message.DateSent.ToDateTime(),
                NotificationType = message.NotificationType,
                SenderId = message.SenderId
            };
            if (curentUserId != default(int))
                messageViewModel.MessageType = message.SenderId == curentUserId ? MessageType.Outgoing : MessageType.Incoming;

            return messageViewModel;
        }

        #endregion


    }
}
