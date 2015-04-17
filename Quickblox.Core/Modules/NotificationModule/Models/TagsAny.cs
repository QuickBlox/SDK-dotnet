using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    /// <summary>
    /// Should contain a string of tags divided by commas. Recipients (users) must have at least one tag that specified in list.
    /// </summary>
    public enum TagsAny
    {
        good, bad, ugly
    }
}
