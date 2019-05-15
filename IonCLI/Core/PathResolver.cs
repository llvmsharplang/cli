using System;
using System.IO;
using IonCLI.Integrity;

namespace IonCLI.Core
{
    /// <summary>
    /// Rigid path utility class that allows
    /// resolving relative paths to a single, root
    /// directory path.
    /// </summary>
    public class PathResolver
    {
        /// <summary>
        /// The root directory path, used within the utility
        /// methods in this class instance.
        /// </summary>
        public Options Options { get; }

        public PathResolver(Options options)
        {
            // Ensure options object is not empty.
            if (options == null)
            {
                throw new ArgumentNullException("Root path cannot be null nor empty");
            }

            this.Options = options;
        }

        /// <summary>
        /// Resolve a path located under the provided
        /// root directory. Returns the absolute path.
        /// </summary>
        public string Resolve(string path)
        {
            // Combine root with provided path.
            string result = Path.Combine(this.Options.Root, path);

            // Resolve full path.
            result = Path.GetFullPath(result);

            // Return the resolved path.
            return result;
        }

        public string Tool(ToolType type)
        {
            // Ensure the tool type provided is valid.
            if (!VerifierConstants.Tools.ContainsKey(type))
            {
                throw new ArgumentException($"Unknown tool type: {type}");
            }

            // Retrive the tool type's corresponding filename.
            string fileName = VerifierConstants.Tools[type].FileName;

            // Resolve tools path.
            string toolsPath = Paths.BaseDirectory(this.Options.ToolsPath);

            // Combine the tools path with the tool's filename.
            string toolPath = Path.Combine(toolsPath, fileName);

            // Return the resulting path.
            return toolPath;
        }

        public string Output(string path)
        {
            // Combine output path with the provided path.
            string result = Path.Combine(this.Options.Output, path);

            // Compute absolute path of the result.
            result = Path.GetFullPath(result);

            // Return resulting path.
            return result;
        }
    }
}
