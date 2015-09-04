using System.Threading.Tasks;

namespace Quickblox.Logger
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
