using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Quickblox.Sdk.AsyncLock;

namespace Quickblox.Logger
{
#if DEBUG || TEST_RELEASE
    internal class FileLogger : ILogger
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
        private readonly AsyncLock mutex = new AsyncLock();

        #endregion

        #region ILogger methods

        public async Task Log(LogLevel logLevel, string message)
        {
            try
            {
                await AppendToFile(LogFileName, string.Format("{0} {1}: {2}{3}", DateTime.Now, logLevel, message, Environment.NewLine));
                Debug.WriteLine("{0}: {1}", logLevel, message);
            }
            catch (Exception) { }
        }

        #endregion

        #region Private methods

        private async Task AppendToFile(string filename, string content)
        {
            byte[] fileBytes = Encoding.UTF8.GetBytes(content.ToCharArray());

            using (await mutex.LockAsync())
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    stream.Position = stream.Length;
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }
            }
        }

        private async Task<string> ReadFile(string filename)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            Stream stream = await local.OpenStreamForReadAsync(filename);
            string text;

            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        #endregion

    }

#endif
}
