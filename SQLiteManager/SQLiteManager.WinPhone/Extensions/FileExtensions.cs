using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SQLiteManager.WinPhone.Extensions
{
    public static class FileExtensions
    {
        /// <summary>
        /// Check if a file exists.
        /// </summary>
        /// <param name="folder">The folder to check if a specific file exists.</param>
        /// <param name="filename">The name of the file to search.</param>
        /// <returns>TRUE = file exists | FALSE = file does not exist.</returns>
        public static async Task<bool> FileExists(this StorageFolder folder, string filename)
        {
            return (await folder.GetFilesAsync()).Any(x => x.Name.Equals(filename));
        }
    }
}
