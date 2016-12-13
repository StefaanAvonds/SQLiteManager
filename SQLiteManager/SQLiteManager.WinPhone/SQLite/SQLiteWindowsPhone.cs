using SQLite;
using SQLiteManager.Interfaces;
using SQLiteManager.WinPhone.FileSystem;
using SQLiteManager.WinPhone.SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteWindowsPhone))]
namespace SQLiteManager.WinPhone.SQLite
{
    public class SQLiteWindowsPhone : ISQLite
    {
        private IDatabaseFile _databaseFile;

        public SQLiteWindowsPhone()
        {
            _databaseFile = new DatabaseFileWindowsPhone();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var databaseFilePath = _databaseFile.GetDatabaseFileLocation();

            var connection = new SQLiteAsyncConnection(databaseFilePath, false);

            return connection;
        }
    }
}
