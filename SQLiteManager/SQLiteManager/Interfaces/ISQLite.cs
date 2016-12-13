using SQLite;

namespace SQLiteManager.Interfaces
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
