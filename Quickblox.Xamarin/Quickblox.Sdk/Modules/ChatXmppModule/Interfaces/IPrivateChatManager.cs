using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Threading.Tasks;

namespace Quickblox.Sdk
{
	public interface IPrivateChatManager
	{
		void SendMessage(string body, string subject = null, string thread = null, MessageType messageType = MessageType.Normal, NotificationType notificationType = NotificationType.None, bool saveHistory = true);
    }
}

