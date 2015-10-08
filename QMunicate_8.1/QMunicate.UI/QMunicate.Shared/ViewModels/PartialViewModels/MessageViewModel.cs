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
        #region Ctor

        public MessageViewModel() { }

        protected MessageViewModel(Message message, int curentUserId = default(int))
        {
            MessageText = message.MessageText;
            DateTime = message.DateSent.ToDateTime();
            if (curentUserId != default(int))
                MessageType = message.SenderId == curentUserId ? MessageType.Outgoing : MessageType.Incoming;

            NotificationType = message.NotificationType;
        }

        #endregion

        #region Properties

        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationTypes NotificationType { get; set; }

        #endregion

        #region Public methods

        public static MessageViewModel FromMessage(Message message, int curentUserId = default(int))
        {
            return new MessageViewModel(message, curentUserId);
        }

        #endregion


    }
}
