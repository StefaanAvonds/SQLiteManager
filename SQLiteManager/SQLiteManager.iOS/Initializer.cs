using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteManager.iOS
{
    /// <summary>
    /// Initializer for iOS. Call this class in AppDelegate before "LoadApplication" to make sure this package works for iOS.
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// Initialize the SQLiteDatabase-package for iOS.
        /// </summary>
        public static void Init()
        {
            // Don't delete this!!!
            // This is only needed for else DependencyService.Get will NOT find the right class
            // The linker of iOS will "delete" every class that the linker thinks is not used

            var sqlite = new SQLiteManager.iOS.SQLite.SQLiteIOS();
            var dtabaseFile = new SQLiteManager.iOS.FileSystem.DatabaseFileIOS();
        }
    }
}
