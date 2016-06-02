using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.ChatXmppModule.Models
{
    public enum JidType
    {
        /// <summary>
        /// Type is unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// User's JID
        /// </summary>
        Private,
        /// <summary>
        /// User's JID in some group chat
        /// </summary>
        Group
    }
}
