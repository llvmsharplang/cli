using System;
using System.Diagnostics;
using System.IO;
using IonCLI.Core;

namespace IonCLI.Engines
{
    public class ExecutionEngine : OperationEngine
    {
        public ExecutionEngine(EngineContext context) : base(context)
        {
            this.Dependencies = new OperationEngine[]
            {
                new BuildEngine(context)
            };
        }

        public override void Invoke()
        {
            // Prepare engine.
            this.Prepare();

            // Retrieve the application's identifier.
            string packageIdentifier = $"{this.context.Package.Identifier}.exe";

            // Resolve the executable's path.
            string executablePath = this.context.Options.PathResolver.Output(packageIdentifier);

            // Ensure executable exists.
            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException("Could not find required output executable");
            }

            // Extract the executable's file name for future
            string executableFileName = Path.GetFileName(executablePath);

            // Prepare the process.
            Process process = new Process();

            // Specify executable path.
            process.StartInfo.FileName = executablePath;

            // Do not create a window.
            process.StartInfo.CreateNoWindow = true;

            // Do not execute from shell.
            process.StartInfo.UseShellExecute = false;

            // Instruct process to redirect output.
            process.StartInfo.RedirectStandardOutput = true;

            // Start the process.
            process.Start();

            // Capture the output of the executable.
            string output = process.StandardOutput.ReadToEnd();

            // Display the executable's output.
            Log.Compose($"\n{output.Trim()}");

            // Wait for completion.
            process.WaitForExit();
        }
    }
}
