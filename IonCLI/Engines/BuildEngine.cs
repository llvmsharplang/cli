using System;
using System.Collections.Generic;
using System.IO;
using Ion.CodeGeneration.Structure;
using IonCLI.Core;
using IonCLI.Integrity;

namespace IonCLI.Engines
{
    internal class BuildEngine : OperationEngine
    {
        public BuildEngine(EngineContext context) : base(context)
        {
            //
        }

        public override void Invoke()
        {
            // Prepare engine.
            this.Prepare();

            // Emit the project.
            Dictionary<string, string> modules = this.context.Project.Emit();

            // Create a new tool invoker instace.
            ToolInvoker toolInvoker = new ToolInvoker(this.context.Options);

            // Create a list of emitted Bitcode files.
            List<string> outputBitcodeFiles = new List<string>();

            // Create a list of emitted IR files.
            List<string> outputIrFiles = new List<string>();

            // Loop through all the emitted results.
            foreach ((string fileName, string output) in modules)
            {
                // Form the final, expected output file name.
                string outputFileName = Path.ChangeExtension(fileName, FileExtension.IR);

                // Resolve output IR file's path.
                string outputIrFilePath = this.context.Options.PathResolver.Output(outputFileName);

                // Append IR file to the emitted list.
                outputIrFiles.Add(outputIrFilePath);

                // Write output IR file.
                File.WriteAllText(outputIrFilePath, output);

                // TODO: Ensure it was created/it exists (IR output file).

                // Invoke the LLC tool to compile to object code (Bitcode).
                toolInvoker.Invoke(ToolType.LLC, new string[]
                {
                    "-filetype=obj",

                    // TODO: Hard-coded target.
                    "-mtriple=x86_64-pc-windows-msvc",
                    outputIrFilePath
                });

                // TODO: Also ensure it was created/it exists (Bitcode output file).

                // Prepare outputted bitcode filename.
                string outputBitcodeFilePath = Path.ChangeExtension(outputIrFilePath, FileExtension.Bitcode);

                // Append Bitcode file to the emitted list.
                outputBitcodeFiles.Add(outputBitcodeFilePath);
            }

            // Compute the executable's filename.
            string outputExecutableFileName = this.GetExecutableFileName();

            // Create the output executable full path.
            string outputExecutablePath = this.context.Options.PathResolver.Output(outputExecutableFileName);

            // Prepare the arguments array.
            string[] args;

            // Determine whether to use Windows configuration.
            if (Util.IsWindowsOs)
            {
                args = this.BuildOnWindows(outputBitcodeFiles, outputExecutablePath, toolInvoker);
            }
            // Otherwise, build on Unix-like.
            else
            {
                args = this.BuildOnUnixLike();
            }

            // Compute the (linker) tool type to use.
            ToolType toolType = this.GetToolType();

            // Invoke the linker with the arguments as an array.
            toolInvoker.Invoke(toolType, args);

            // Invoke the cleanup method.
            this.Cleanup(outputIrFiles, outputBitcodeFiles);

            // Ensure program was compiled successful.
            if (!File.Exists(outputExecutablePath))
            {
                Log.Error("Could not create output executable.");
            }

            // Inform the user that the compilation was successfull.
            Log.Success("Compilation successful.");
        }

        protected string GetExecutableFileName()
        {
            string fileName = this.context.Package.Identifier;

            // Append an executable extension if on Windows.
            if (Util.IsWindowsOs)
            {
                fileName += $".{FileExtension.WindowsExecutable}";
            }

            // Return the resulting filename.
            return fileName;
        }

        protected ToolType GetToolType()
        {
            // Use Link on Windows.
            if (Util.IsWindowsOs)
            {
                return ToolType.Link;
            }

            // Otherwise, use LLD.
            return ToolType.LLD;
        }

        protected string[] BuildOnWindows(List<string> outputBitcodeFiles, string outputExecutablePath, ToolInvoker toolInvoker)
        {
            // Resolve the Link tool's root path.
            string linkToolRoot = this.context.Options.PathResolver.ToolRoot(ToolType.Link);

            // TODO: Hard-coded for Windows.
            // Create the argument list for LLD.
            List<string> args = new List<string>
            {
                "/DEFAULTLIB:libcmt",
                $"/OUT:{outputExecutablePath}",
                $"/LIBPATH:{linkToolRoot}"
            };

            // Append all emitted bitcode files.
            args.AddRange(outputBitcodeFiles);

            // Return the resulting arguments.
            return args.ToArray();
        }

        protected string[] BuildOnUnixLike()
        {
            throw new NotImplementedException();
        }

        protected void Cleanup(List<string> outputIrFiles, List<string> outputBitcodeFiles)
        {
            // Determine if emitted files should be kept.
            if (!this.context.Options.KeepEmittedFiles)
            {
                // Inform the user that cleaning process has begun.
                Log.Verbose("Cleaning up output files.");

                // Cleanup emitted IR files.
                foreach (string outputIrFile in outputIrFiles)
                {
                    // Delete the file.
                    File.Delete(outputIrFile);
                }

                // Cleanup emitted Bitcode files.
                foreach (string outputBitcodeFile in outputBitcodeFiles)
                {
                    // Delete the file.
                    File.Delete(outputBitcodeFile);
                }
            }
            // Otherwise, inform the user.
            else
            {
                // Inform the user that the cleaning process will not be executed.
                Log.Verbose("Keeping emitted files.");
            }
        }
    }
}
