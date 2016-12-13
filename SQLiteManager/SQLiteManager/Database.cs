using SQLite;
using SQLiteManager.Interfaces;
using System;
using Xamarin.Forms;

namespace SQLiteManager
{
    public static class Database
    {
        private static SQLiteAsyncConnection _asyncConnection;
        private static IDatabaseFile _databaseFile;

        /// <summary>
        /// The asynchronous connection to the SQLite-database.
        /// This connection is used by every DataAccess-class throughout.
        /// </summary>
        public static SQLiteAsyncConnection AsyncConnection
        {
            get
            {
                if (_asyncConnection == null) _asyncConnection = DependencyService.Get<ISQLite>().GetAsyncConnection();
                return _asyncConnection;
            }
        }

        /// <summary>
        /// The default filename for the SQLite-database.
        /// </summary>
        public static String DefaultDatabaseFilename = "my_database.db3";

        /// <summary>
        /// The default filename for the exported file.
        /// </summary>
        public static String DefaultExportFilename = "Database.db3";

        /// <summary>
        /// Initialize the DatabaseFile-object.
        /// </summary>
        private static void InitializeDatabaseFileObject()
        {
            if (_databaseFile == null) _databaseFile = DependencyService.Get<IDatabaseFile>();
        }

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        public static bool TryExportDatabase(out string path)
        {
            InitializeDatabaseFileObject();
            return _databaseFile.TryExportDatabase(out path);
        }

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <param name="exportFilename">The expected filename of the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        public static bool TryExportDatabase(out string path, string exportFilename)
        {
            InitializeDatabaseFileObject();
            return _databaseFile.TryExportDatabase(out path, exportFilename);
        }

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="databaseFilename">The name of the SQLite-file.</param>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        public static bool TryExportDatabase(string databaseFilename, out string path)
        {
            InitializeDatabaseFileObject();
            return _databaseFile.TryExportDatabase(databaseFilename, out path);
        }

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="databaseFilename">The name of the SQLite-file.</param>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <param name="exportFilename">The expected filename of the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        public static bool TryExportDatabase(string databaseFilename, out string path, string exportFilename)
        {
            InitializeDatabaseFileObject();
            return _databaseFile.TryExportDatabase(databaseFilename, out path, exportFilename);
        }
    }
}
