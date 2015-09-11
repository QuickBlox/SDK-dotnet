using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Quickblox.Sdk.Logger;

namespace QMunicate.Logger
{
    public class FileLogger : ILogger //, IQmunicateLogger
    {
        public async Task Log(LogLevel logLevel, string message)
        {
            Debug.WriteLine(message);
        }
    }
}
