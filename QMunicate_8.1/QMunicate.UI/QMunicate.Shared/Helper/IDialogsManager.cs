using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk.Modules.Models;

namespace QMunicate.Helper
{
    public interface IDialogsManager
    {
        List<Dialog> Dialogs { get; set; }
        Task ReloadDialogs();
    }
}
