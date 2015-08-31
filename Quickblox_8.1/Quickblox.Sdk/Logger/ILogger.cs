using System.Threading.Tasks;

namespace Quickblox.Logger
{
#if TEST_RELEASE || DEBUG
    internal interface ILogger
    {
        Task Log(LogLevel logLevel, string message);
    }

    internal enum LogLevel
    {
        Error,
        Warn,
        Debug
    }
#endif
}
