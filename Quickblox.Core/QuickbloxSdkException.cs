using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Sdk
{
    public class QuickbloxSdkException : Exception
    {
        public QuickbloxSdkException()
        {
        }

        public QuickbloxSdkException(string message)
            : base(message)
        {
        }

        public QuickbloxSdkException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
