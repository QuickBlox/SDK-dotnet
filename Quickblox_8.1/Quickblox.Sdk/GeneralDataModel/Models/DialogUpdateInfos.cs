using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.GeneralDataModel.Models
{
    /// <summary>
    /// Information about what was modified in a dialog. Is sent as an extra param.
    /// </summary>
    public enum DialogUpdateInfos
    {
        UpdatedDialogPhoto = 1,
        UpdatedDialogName = 2,
        ModifiedOccupantsList = 3
    }

    public static class DialogUpdateInfosExtentions
    {
        public static string ToIntString(this DialogUpdateInfos dialogUpdateInfo)
        {
            return ((int)dialogUpdateInfo).ToString();
        }
    }
}
