using Quickblox.Sdk.Modules.ChatXmppModule.Models;
using System.Threading.Tasks;

namespace Quickblox.Sdk
{
	public interface IPrivateChatManager
	{
		void SendMessage(string body, string subject = null, string thread = null, MessageType messageType = MessageType.Normal, NotificationType notificationType = NotificationType.None);

        Task<bool> AddToFriends(RosterItem item);

        bool AcceptFriend(RosterItem item);

        bool RejectFriend(RosterItem item);

        Task<bool> RemoveFriend(RosterItem item);
    }
}

