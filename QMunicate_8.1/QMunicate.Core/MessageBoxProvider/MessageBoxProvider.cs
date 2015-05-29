using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace QMunicate.Core.MessageBoxProvider
{
    public class MessageBoxProvider : IMessageBoxProvider
    {
        public async Task<MessageBoxResult> ShowAsync(string title, string content = null, MessageBoxButton messageBoxButton = MessageBoxButton.Ok)
        {
            var md = new MessageDialog(content ?? "", title);
            bool result = false;
            md.Commands.Add(new UICommand("Ok", (cmd) => result = true));

            if (messageBoxButton == MessageBoxButton.OkCancel)
                md.Commands.Add(new UICommand("Cancel", (cmd) => result = false));

            await md.ShowAsync();
            return result ? MessageBoxResult.Ok : MessageBoxResult.Cancel;
        }
    }
}
