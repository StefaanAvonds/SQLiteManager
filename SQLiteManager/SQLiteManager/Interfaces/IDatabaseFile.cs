namespace SQLiteManager.Interfaces
{
    public interface IDatabaseFile
    {
        /// <summary>
        /// Get the location of the SQLite-database.
        /// </summary>
        /// <returns>The full path to the SQLite-file.</returns>
        string GetDatabaseFileLocation();

        /// <summary>
        /// Get the location of the SQLite-database.
        /// </summary>
        /// <param name="databaseFilename">The name of the SQLite-file.</param>
        /// <returns>The full lpath to the SQLite-file.</returns>
        string GetDatabaseFileLocation(string databaseFilename);

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        bool TryExportDatabase(out string path);

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <param name="exportFilename">The expected filename of the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        bool TryExportDatabase(out string path, string exportFilename);

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="databaseFilename">The name of the SQLite-file.</param>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        bool TryExportDatabase(string databaseFilename, out string path);

        /// <summary>
        /// Try to export the SQLite-database to a local path.
        /// </summary>
        /// <param name="databaseFilename">The name of the SQLite-file.</param>
        /// <param name="path">The full path to the exported database-file.</param>
        /// <param name="exportFilename">The expected filename of the exported database-file.</param>
        /// <returns>TRUE = export has been done | FALSE = export has failed</returns>
        bool TryExportDatabase(string databaseFilename, out string path, string exportFilename);
    }
}
