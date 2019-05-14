using System;
using System.Diagnostics;
using System.IO;
using IonCLI.Core;
using IonCLI.Integrity;

namespace Ion.Core
{
    internal sealed class ToolInvoker
    {
        private readonly Options options;

        public ToolInvoker(Options options)
        {
            this.options = options;
        }

        public void Invoke(ToolType toolType, string[] args)
        {
            // Create a new process instance.
            Process process = new Process();

            // Ensure that the corresponding tool definition exists.
            if (!VerifierConstants.Tools.ContainsKey(toolType))
            {
                throw new Exception("Tool definition for the provided tool type does not exist");
            }

            // Retrieve the tool definition.
            ToolDefinition tool = VerifierConstants.Tools[toolType];

            // Construct tool path.
            string toolPath = Path.Combine(this.options.ToolsPath, tool.FileName);

            // Resolve the tool's executable path.
            string resolvedToolPath = this.options.PathResolver.Resolve(toolPath);

            // Set the process' target file.
            process.StartInfo.FileName = resolvedToolPath;

            // Do not create a window.
            process.StartInfo.CreateNoWindow = true;

            // Execute from shell.
            process.StartInfo.UseShellExecute = true;

            // Start the process.
            process.Start();
        }

        public void Invoke(ToolType tool)
        {
            this.Invoke(tool, new string[] { });
        }
    }
}
