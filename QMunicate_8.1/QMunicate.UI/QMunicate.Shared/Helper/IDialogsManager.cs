using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using QMunicate.Models;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Helper
{
    public interface IDialogsManager
    {
        ObservableCollection<DialogVm> Dialogs { get; }
        Task ReloadDialogs();
        void JoinAllGroupDialogs();
        Task UpdateDialogLastMessage(string dialogId, string lastActivity, DateTime lastMessageSent);
    }
}
