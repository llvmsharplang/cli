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

            // Inform the user that the program is being run.
            Log.Verbose("Running output executable.");

            // Start the process.
            process.Start();

            // Capture the output of the executable.
            string output = process.StandardOutput.ReadToEnd();

            // Display the executable's output.
            Log.Compose(output.Trim());

            // Retrieve the exit code.
            int exitCode = process.ExitCode;

            // Create the output exit code message.
            string exitCodeMessage = $"Program exited with code {exitCode}.";

            // Exit code was not successful.
            if (exitCode != 0 && !this.context.Options.IgnoreExitCode)
            {
                // Log the error and terminate program.
                Log.Error(exitCodeMessage, exitCode);
            }
            // Otherwise the program was successful.
            else
            {
                // Inform the user of the program's successful exit code.
                Log.Verbose(exitCodeMessage);
            }

            // Wait for completion.
            process.WaitForExit();
        }
    }
}
