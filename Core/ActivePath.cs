using System.IO;

namespace IonCLI.Core
{
    /// <summary>
    /// Rigid path utility class that allows
    /// resolving relative paths to a single, root
    /// directory path.
    /// </summary>
    public class ActivePath
    {
        /// <summary>
        /// The root directory path, used within the utility
        /// methods in this class instance.
        /// </summary>
        public string Root { get; }

        public ActivePath(string root)
        {
            this.Root = root;
        }

        /// <summary>
        /// Resolve a path located under the provided
        /// root directory.
        /// </summary>
        public string Resolve(string path)
        {
            return Path.Combine(this.Root, path);
        }
    }
}
