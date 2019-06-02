using System;
using System.IO;
using IonCLI.Tools;
using IonCLI.Integrity;
using IonCLI.InterOperability;

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

        public string ToolRoot(ToolType type)
        {
            // Ensure the tool type provided is valid.
            if (!ToolConstants.Tools.ContainsKey(type))
            {
                throw new ArgumentException($"Unknown tool type: {type}");
            }

            // Retrieve the tool.
            ToolDefinition tool = ToolConstants.Tools[type];

            // Resolve tools path.
            string toolsPath = Paths.BaseDirectory(this.Options.ToolsPath);

            // Initialize the tool's root path to be the previously resolved tools path.
            string toolRootPath = toolsPath;

            // Prepare the tool's root path if applicable.
            if (tool.Platforms != null)
            {
                // Combine the tools path with the tool's root path.
                toolRootPath = Path.Combine(toolsPath, Platform.Id);
            }

            // Append the sub path at this point, if applicable.
            if (tool.SubPath != null)
            {
                toolRootPath = Path.Combine(toolRootPath, tool.SubPath);
            }

            // Return the resulting tool's root path.
            return toolRootPath;
        }

        public string ToolResource(ToolType type, string resourcePath)
        {
            // Resolve the tool's root folder and combine it with the resources folder path.
            string resourcesPath = this.ToolResourcesFolder(type);

            // Combine the tool's root with the provided resource's path.
            string result = Path.Combine(resourcesPath, resourcePath);

            // Return the resulting resource path.
            return result;
        }

        public string ToolResourcesFolder(ToolType type)
        {
            return Path.Combine(this.ToolRoot(type), ToolConstants.ResourcesFolder);
        }

        public string Tool(ToolType type)
        {
            // Ensure the tool type provided is valid.
            if (!ToolConstants.Tools.ContainsKey(type))
            {
                throw new ArgumentException($"Unknown tool type: {type}");
            }

            // Retrieve the tool.
            ToolDefinition tool = ToolConstants.Tools[type];

            // Retrive the tool type's corresponding filename.
            string fileName = tool.FileName;

            // TODO: Subtly hard-coded.
            // Append the executable extension if on Windows.
            if (Util.IsWindowsOS)
            {
                fileName += $".{FileExtension.WindowsExecutable}";
            }

            // Retrieve the tool's root path.
            string toolRootPath = this.ToolRoot(type);

            // Combine the tools path (or root path) with the tool's filename.
            string toolPath = Path.Combine(toolRootPath, fileName);

            // Resolve the tool path into an absolute path.
            toolPath = Path.GetFullPath(toolPath);

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
