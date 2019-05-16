using System;
using System.IO;

namespace IonCLI.Core
{
    public static class Paths
    {
        /// <summary>
        /// Resolve a path under the base directory,
        /// or the directory path in which the application's
        /// executable is located.
        /// </summary>
        public static string BaseDirectory(string path)
        {
            // Merge the base path with the provided path.
            string result = Path.Combine(AppContext.BaseDirectory, path);

            // Resolve the path into an absolute path.
            result = Path.GetFullPath(result);

            // Return the resulting path.
            return result;
        }

        /// <summary>
        /// Resolve a path under the current working directory,
        /// or the directory path from which the application
        /// was executed.
        /// </summary>
        public static string WorkingDirectory(string path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }
    }
}
