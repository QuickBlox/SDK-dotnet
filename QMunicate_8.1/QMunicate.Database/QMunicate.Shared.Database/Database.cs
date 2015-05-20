using System.IO;
using Windows.Storage;

namespace QMunicate.Database
{
    internal class Database
    {
        #region Ctor

        /// <summary>
        /// </summary>
        /// <param name="storageType"></param>
        public Database(StorageType storageType)
        {
            StorageType = storageType;
            StorageFolder = Helper.GetFolder(storageType);
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public StorageType StorageType { get; private set; }

        /// <summary>
        /// </summary>
        public StorageFolder StorageFolder { get; private set; }

        /// <summary>
        ///     Get an open connection to a SQLite database.
        /// </summary>
        internal SQLiteConnection Connection { get; private set; }

        #endregion

        #region Internal Members

        /// <summary>
        /// </summary>
        internal void Initialize()
        {
            // Generate database path
            string databasePath = Path.Combine(StorageFolder.Path, Helper.DATABASE);
            // Constructs a new connection and opens a SQLite database specified by databasePath
            Connection = new SQLiteConnection(databasePath);
        }

        #endregion

        /// <summary>
        /// </summary>
        internal class DatabaseAsync
        {
            #region Ctor

            /// <summary>
            /// </summary>
            /// <param name="storageType"></param>
            public DatabaseAsync(StorageType storageType)
            {
                StorageType = storageType;
                StorageFolder = Helper.GetFolder(storageType);
                Initialize();
            }

            #endregion

            #region Properties

            /// <summary>
            /// </summary>
            public StorageType StorageType { get; private set; }

            /// <summary>
            /// </summary>
            public StorageFolder StorageFolder { get; private set; }

            /// <summary>
            ///     Get an open connection to a SQLite database.
            /// </summary>
            internal SQLiteAsyncConnection AsyncConnection { get; set; }

            #endregion

            #region Private Members

            /// <summary>
            /// </summary>
            private void Initialize()
            {
                var databasePath = Path.Combine(StorageFolder.Path, Helper.DATABASE);
                // Constructs a new connection and opens a SQLite database specified by databasePath
                AsyncConnection = new SQLiteAsyncConnection(databasePath);
            }

            #endregion
        }
    }
}
