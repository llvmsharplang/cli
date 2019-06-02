using System;
using System.Diagnostics;
using System.IO;
using Ion.Core;
using IonCLI.Core;
using IonCLI.Integrity;
using IonCLI.Tools;

namespace IonCLI.Core
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
            // Ensure that the corresponding tool definition exists.
            if (!ToolConstants.Tools.ContainsKey(toolType))
            {
                throw new Exception("Tool definition for the provided tool type does not exist");
            }

            // Retrieve the tool definition.
            ToolDefinition tool = ToolConstants.Tools[toolType];

            // Resolve the tool's executable path.
            string resolvedToolPath = this.options.PathResolver.Tool(toolType);

            // Inform the user that tool's path verification is taking place.
            Log.Verbose($"Verifying tool '{tool.FileName}' path: {resolvedToolPath}");

            // Ensure tool path exists.
            if (!File.Exists(resolvedToolPath))
            {
                Log.Error($"Tool '{tool.FileName}' does not exist.");
            }

            // Otherwise, inform the user that the tool path exists.
            Log.Verbose($"Tool path for '{tool.FileName}' exists.");

            // Create the runnable object instance.
            Runnable runnable = new Runnable
            {
                Path = resolvedToolPath,
                Arguments = args
            };

            // Inform the user of the tool being started.
            Log.Verbose($"Invoking tool: {tool.FileName}");

            // Invoke the runnable.
            string output = runnable.Run();

            // Process the tool output.
            Log.Output(output, tool.FileName);
        }

        public void Invoke(ToolType tool)
        {
            this.Invoke(tool, new string[] { });
        }
    }
}
