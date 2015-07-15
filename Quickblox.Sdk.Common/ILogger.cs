using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Common
{
    public interface ILogger
    {
        Task Log(LogLevel logLevel, string message);
    }

    public enum LogLevel
    {
        Error,
        Warn,
        Debug
    }
}
