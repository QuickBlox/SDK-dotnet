using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk
{
	public interface IPrivateChatManager
	{
		void SendMessage(string body, string subject = null, string thread = null, MessageType messageType = MessageType.Normal, NotificationType notificationType = NotificationType.None);
	}
}

