using SQLite;
using SQLiteManager.Interfaces;
using SQLiteManager.Windows.FileSystem;
using SQLiteManager.Windows.SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteWindows))]
namespace SQLiteManager.Windows.SQLite
{
    public class SQLiteWindows : ISQLite
    {
        private IDatabaseFile _databaseFile;

        public SQLiteWindows()
        {
            _databaseFile = new DatabaseFileWindows();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var databaseFilePath = _databaseFile.GetDatabaseFileLocation();

            var connection = new SQLiteAsyncConnection(databaseFilePath, false);

            return connection;
        }
    }
}
