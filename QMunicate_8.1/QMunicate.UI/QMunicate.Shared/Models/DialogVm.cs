using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Quickblox.Sdk.Builder;
using Quickblox.Sdk.Modules.ChatModule.Models;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Models
{
    public class DialogVm
    {
        #region Ctor

        public DialogVm()
        {
        }

        protected DialogVm(Dialog dialog)
        {
            Id = dialog.Id;
            Name = dialog.Name;
            LastMessageSent = dialog.LastMessageDateSent.HasValue
                ? dialog.LastMessageDateSent.Value.ToDateTime()
                : (DateTime?) null;
            LastActivity = dialog.LastMessage;
            UnreadMessageCount = dialog.UnreadMessagesCount;
            Image = dialog.Photo;
            OccupantIds = dialog.OccupantsIds;
            DialogType = dialog.Type;
        }

        #endregion

        #region Properties

        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public DateTime? LastMessageSent { get; set; }
        public string LastActivity { get; set; }
        public int? UnreadMessageCount { get; set; }
        public IList<int> OccupantIds { get; set; }
        public DialogType DialogType { get; set; }

        #endregion

        #region Public methods

        public static DialogVm FromDialog(Dialog dialog)
        {
            return new DialogVm(dialog);
        }

        #endregion
    }
}
