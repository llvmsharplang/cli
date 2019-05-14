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

            // Resolve the tool's executable path.
            string resolvedToolPath = this.options.PathResolver.Tool(toolType);

            // Ensure tool path exists.
            if (!File.Exists(resolvedToolPath))
            {
                Log.Error($"Tool '{tool.FileName}' does not exist.");
            }

            // Otherwise, inform the user that the tool path exists.
            Log.Verbose($"Tool path for '{tool.FileName}' exists.");

            // Set the process' target file.
            process.StartInfo.FileName = resolvedToolPath;

            // Do not create a window.
            process.StartInfo.CreateNoWindow = true;

            // Do not execute from shell.
            process.StartInfo.UseShellExecute = false;

            // Fill all provided arguments.
            foreach (string arg in args)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            // Instruct process to redirect output.
            process.StartInfo.RedirectStandardOutput = true;

            // TODO: Finish implementing.
            process.OutputDataReceived += (sender, data) =>
            {
                System.Console.WriteLine($"Tool '{tool.FileName}' standard output: {data.Data}");
            };

            // TODO: Finish implementing.
            process.ErrorDataReceived += (sender, data) =>
            {
                System.Console.WriteLine($"Tool '{tool.FileName}' error output: {data.Data}");
            };

            // Instruct process to redirect errors.
            process.StartInfo.RedirectStandardError = true;

            // Inform the user of the tool being started.
            Log.Verbose($"Invoking tool: {tool.FileName}");

            // Start the process.
            process.Start();

            // Inform the user of the waiting state.
            Log.Verbose($"Awaiting tool: {tool.FileName}");

            // Wait for completion.
            process.WaitForExit();
        }

        public void Invoke(ToolType tool)
        {
            this.Invoke(tool, new string[] { });
        }
    }
}
