using Quickblox.Sdk.Logger;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Test.Logger
{
    public class DebugLogger : ILogger
    {
        public async Task Log(LogLevel logLevel, string message)
        {
            Debug.WriteLine("{0}: {1}", logLevel, message);
        }
    }
}
