using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Modules.ChatModule.Models
{
    /// <summary>
    /// lt (Less Than operator), 
    /// lte (Less Than or Equal to operator), 
    /// gt (Greater Than operator), 
    /// gte (Greater Than or Equal to operator), 
    /// ne (Not Equal to operator),
    /// in (Contained IN array operator), 
    /// nin (Not contained IN array), 
    /// all (ALL contained IN array),
    /// or (OR operator),
    /// ctn (Contains substring operator)
    /// </summary>
    public enum DialogSearchOperator
    {
        Lt,
        Lte,
        Gt,
        Gte,
        Ne,
        In,
        Nin,
        All,
        Or,
        Ctn
    }
}
