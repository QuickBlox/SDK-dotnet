using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Quickblox.Sdk.Common;

namespace Quickblox.Sdk
{
    public sealed class QuickbloxLogger : ILogger
    {
        #region Singleton

        private static readonly QuickbloxLogger instance = new QuickbloxLogger();

        private QuickbloxLogger()
        {
        }

        public static QuickbloxLogger Instance
        {
            get { return instance; }
        }

        #endregion

        public ILogger LoggerProvider { get; set; }

        /// <summary>
        /// Logs a specified message with specified LoggerProvider.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Log(LogLevel logLevel, string message)
        {
#if TEST_RELEASE || DEBUG
            var loggerProvider = LoggerProvider;
            if (loggerProvider == null) return;

            await loggerProvider.Log(logLevel, message);
#endif
        }
    }

    
}
