using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk.Logger
{
    internal class LoggerHolder
    {
        public static ILogger LoggerInstance { get; set; }

        public static async Task Log(LogLevel logLevel, string message)
        {
            var logger = LoggerInstance;
            if (logger != null)
            {
                await logger.Log(logLevel, message);
            }
        }
    }
}
