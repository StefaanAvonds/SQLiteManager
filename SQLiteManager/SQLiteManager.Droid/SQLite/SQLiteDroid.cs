using SQLite;
using SQLiteManager.Droid.FileSystem;
using SQLiteManager.Droid.SQLite;
using SQLiteManager.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDroid))]
namespace SQLiteManager.Droid.SQLite
{
    public class SQLiteDroid : ISQLite
    {
        private IDatabaseFile _databaseFile;

        public SQLiteDroid()
        {
            _databaseFile = new DatabaseFileDroid();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var databaseFilePath = _databaseFile.GetDatabaseFileLocation();

            var connection = new SQLiteAsyncConnection(databaseFilePath, false);

            return connection;
        }
    }
}