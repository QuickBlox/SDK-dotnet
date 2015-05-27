using System;
using Windows.UI.Xaml.Media;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Models
{
    public class DialogVm
    {
        public ImageSource Image { get; set; }
        public string Name { get; set; }
        public DateTime? LastMessageSent { get; set; }
        public string LastActivity { get; set; }
        public int? UnreadMessageCount { get; set; }

        public static explicit operator DialogVm(Dialog dialog)
        {
            return new DialogVm()
            {
                Name = dialog.Name,
                LastMessageSent = dialog.LastMessageDateSent.HasValue ? dialog.LastMessageDateSent.Value.ToDateTime() : (DateTime?)null,
                LastActivity = dialog.LastMessage,
                UnreadMessageCount = dialog.UnreadMessagesCount
            };
        }
    }
}
