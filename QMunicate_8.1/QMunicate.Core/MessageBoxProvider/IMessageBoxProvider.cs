using System.Threading.Tasks;

namespace QMunicate.Core.MessageBoxProvider
{
    public interface IMessageBoxProvider
    {
        Task<MessageBoxResult> ShowAsync(string title, string content = null, MessageBoxButton messageBoxButton = MessageBoxButton.Ok);
    }
}
