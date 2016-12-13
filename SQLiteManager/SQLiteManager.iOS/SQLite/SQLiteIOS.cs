using SQLite;
using SQLiteManager.Interfaces;
using SQLiteManager.iOS.FileSystem;
using SQLiteManager.iOS.SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteIOS))]
namespace SQLiteManager.iOS.SQLite
{
    public class SQLiteIOS : ISQLite
    {
        private IDatabaseFile _databaseFile;

        public SQLiteIOS()
        {
            _databaseFile = new DatabaseFileIOS();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var databaseFilePath = _databaseFile.GetDatabaseFileLocation();

            var connection = new SQLiteAsyncConnection(databaseFilePath, false);

            return connection;
        }
    }
}
