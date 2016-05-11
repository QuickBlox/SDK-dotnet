using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk
{
    /// <summary>
    /// Base Quickblox SDK exception.
    /// </summary>
    public class QuickbloxSdkException : Exception
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public QuickbloxSdkException()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message">Exception Message</param>
        public QuickbloxSdkException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="inner">Inner exception</param>
        public QuickbloxSdkException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
