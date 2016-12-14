using SQLite;

namespace SQLiteManager.Interfaces
{
    /// <summary>
    /// Perform platform-specific actions for SQLite.
    /// </summary>
    public interface ISQLite
    {
        /// <summary>
        /// Get the asynchronous connection to the local SQLite-database.
        /// </summary>
        /// <returns></returns>
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
