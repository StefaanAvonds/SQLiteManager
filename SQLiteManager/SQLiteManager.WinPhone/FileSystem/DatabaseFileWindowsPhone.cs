using SQLiteManager.Interfaces;
using SQLiteManager.WinPhone.Extensions;
using SQLiteManager.WinPhone.FileSystem;
using System;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseFileWindowsPhone))]
namespace SQLiteManager.WinPhone.FileSystem
{
    public class DatabaseFileWindowsPhone : IDatabaseFile
    {
        public string GetDatabaseFileLocation()
        {
            return GetDatabaseFileLocation(Database.DefaultDatabaseFilename);
        }

        public string GetDatabaseFileLocation(string databaseFilename)
        {
            var documentsPath = ApplicationData.Current.LocalFolder.Path;
            var fullPath = System.IO.Path.Combine(documentsPath, databaseFilename);
            return fullPath;
        }

        public bool TryExportDatabase(out string path)
        {
            return TryExportDatabase(Database.DefaultDatabaseFilename, out path);
        }

        public bool TryExportDatabase(out string path, string exportFilename)
        {
            return TryExportDatabase(Database.DefaultDatabaseFilename, out path, exportFilename);
        }

        public bool TryExportDatabase(string databaseFilename, out string path)
        {
            return TryExportDatabase(databaseFilename, out path, Database.DefaultExportFilename);
        }

        public bool TryExportDatabase(string databaseFilename, out string path, string exportFilename)
        {
            path = String.Empty;

            // First we need to get the database itself
            var database = GetDatabaseFileLocation(databaseFilename);

            // If this database-file is not found, the export has failed
            bool exists = ApplicationData.Current.LocalFolder.FileExists(database).Result;
            if (!exists) return false;


            // Since the database-file exists, we need to initialize a path for the exported file
            var export = GetExportFileLocation(exportFilename);

            // If this file already exists, just add the current timestamp to the filename
            exists = ApplicationData.Current.LocalFolder.FileExists(export).Result;
            if (exists) export = GetExportFileLocation(exportFilename, DateTime.Now.ToLocalTime().ToString("yyyyMMdd"));
            // If file still already exists, add a random guid to the original filename
            exists = ApplicationData.Current.LocalFolder.FileExists(export).Result;
            if (exists) export = GetExportFileLocation(exportFilename, Guid.NewGuid().ToString());

            // And ofcourse do the export itself
            var sqliteFile = ApplicationData.Current.LocalFolder.GetFileAsync(databaseFilename).GetResults();
            sqliteFile.CopyAsync(ApplicationData.Current.LocalFolder.GetFolderAsync(export).GetResults()).GetResults();

            // Return true to indicate the export has succeeded
            return true;
        }

        /// <summary>
        /// Create a file-path location for the file to export.
        /// </summary>
        /// <param name="exportFilename">The name of the exported file.</param>
        /// <param name="extra">An extra part that will be placed between the filename and the extension.</param>
        /// <returns>The full path for the export-file.</returns>
        private string GetExportFileLocation(string exportFilename, string extra = "")
        {
            // If the given export filename is empty, use the default filename
            if (String.IsNullOrWhiteSpace(exportFilename)) exportFilename = Database.DefaultExportFilename;
            // If the given export filename does not contain a dot, we assume no extension is given and we provide one
            if (!exportFilename.Contains(".")) exportFilename = $"{exportFilename}.db3";
            if (!String.IsNullOrWhiteSpace(extra))
            {
                // If an extra parameter has been given for the filename
                // this extra must be placed between the given filename and the last point
                // (= before the extension of the file)
                var firstPart = Path.GetFileNameWithoutExtension(exportFilename);
                var lastPart = Path.GetExtension(exportFilename);
                exportFilename = $"{firstPart}_{extra}.{lastPart}";
            }

            // Now combine the Downloads-folder with the result of the export filename
            var downloadsPath = ApplicationData.Current.LocalFolder.Path;
            var fullPath = Path.Combine(downloadsPath, exportFilename);
            return fullPath;
        }
    }
}
