using System;
using Windows.Storage;

namespace QMunicate.Database
{
    public static class Helper
    {
        /// <summary>
        /// </summary>
        internal const String DATABASE = "qmunicate.db";

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StorageFolder GetFolder(StorageType type)
        {
            switch (type)
            {
                case StorageType.Roaming:
                    return ApplicationData.Current.RoamingFolder;
                case StorageType.Local:
                    return ApplicationData.Current.LocalFolder;
                case StorageType.Temporary:
                    return ApplicationData.Current.TemporaryFolder;
                default:
                    throw new Exception(String.Format("Unknown StorageType: {0}", type));
            }
        }
    }
}
