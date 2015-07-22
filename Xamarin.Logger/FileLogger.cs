﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickblox.Logger
{
    public class FileLogger : ILogger
    {
        #region Singleton

        private static readonly FileLogger instance = new FileLogger();

        private FileLogger()
        {
        }

        public static FileLogger Instance
        {
            get { return instance; }
        }

        #endregion

        #region Fields

        private const string LogFileName = "Logs.txt";

        #endregion

        #region ILogger methods

        public async Task Log(LogLevel logLevel, string message)
        {
        }

        #endregion

    }
}