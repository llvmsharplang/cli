using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ion.CodeGeneration.Helpers;
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
                // Print output if standard output mode is enabled.
                if (this.context.Options.PrintOutputIr)
                {
                    Log.Output(output, fileName);
                }

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
                toolInvoker.Invoke(ToolType.Llc, this.GetBuildToolArgs(outputIrFilePath));

                // TODO: Also ensure it was created/it exists (Bitcode output file).

                // Determine the output bitcode file extension based on the OS.
                string extension = Util.IsWindowsOS ? FileExtension.BitcodeObj : FileExtension.BitcodeO;

                // Prepare outputted bitcode filename.
                string outputBitcodeFilePath = Path.ChangeExtension(outputIrFilePath, extension);

                // Append Bitcode file to the emitted list.
                outputBitcodeFiles.Add(outputBitcodeFilePath);
            }

            // Compute the executable's filename.
            string outputExecutableFileName = this.GetExecutableFileName();

            // Create the output executable full path.
            string outputExecutablePath = this.context.Options.PathResolver.Output(outputExecutableFileName);

            // Prepare the linker's arguments array.
            string[] linkerArgs;

            // TODO: Use wrapper for this.
            // Determine whether to use Windows configuration.
            if (Util.IsWindowsOS)
            {
                linkerArgs = this.LinkWindowsTarget(outputBitcodeFiles, outputExecutablePath);
            }
            // Otherwise, build on Unix-like.
            else
            {
                linkerArgs = this.LinkLinuxTarget(outputBitcodeFiles, outputExecutablePath);
            }

            // Compute the (linker) tool type to use.
            ToolType toolType = this.GetLinkToolType();

            // Invoke the linker with the arguments as an array.
            toolInvoker.Invoke(toolType, linkerArgs);

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

        // TODO: Missing support for MacOS.
        protected string[] GetBuildToolArgs(string outputIrFilePath)
        {
            if (Util.IsWindowsOS)
            {
                return this.BuildWindows(outputIrFilePath);
            }

            return this.BuildLinux(outputIrFilePath);
        }

        protected string GetExecutableFileName()
        {
            // Abstract the package's identifier.
            string fileName = this.context.Package.Identifier;

            // Append an executable extension if on Windows.
            if (Util.IsWindowsOS)
            {
                fileName += $".{FileExtension.WindowsExecutable}";
            }

            // Return the resulting filename.
            return fileName;
        }

        protected ToolType GetLinkToolType()
        {
            // Use Link on Windows.
            if (Util.IsWindowsOS)
            {
                return ToolType.WindowsLink;
            }

            // Otherwise, use LD.LLD.
            return ToolType.LinuxLdLld;
        }

        protected string[] LinkWindowsTarget(List<string> outputBitcodeFiles, string outputExecutablePath)
        {
            // Resolve the Link tool's root path.
            string linkToolRoot = this.context.Options.PathResolver.ToolRoot(ToolType.WindowsLink);

            // Create the argument list for the Link tool.
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

        protected string[] LinkLinuxTarget(List<string> outputBitcodeFiles, string outputExecutablePath)
        {
            // Define the tool type to use.
            ToolType tool = ToolType.GenericLld;

            // Resolve the Unix-like LLD tool's root path.
            string toolRoot = this.context.Options.PathResolver.ToolRoot(tool);

            // Join all output bitcode files onto a string.
            string targetFiles = string.Join(" ", outputBitcodeFiles);

            // Abstract the path resolver from the context's options.
            PathResolver resolver = this.context.Options.PathResolver;

            // TODO: Finish implementation.
            // Prepare the arguments.
            List<string> args = new List<string>
            {
                "-m",
                "elf_x86_64",
                "-dynamic-linker",
                resolver.ToolResource(tool, "ld-2.27.so"),
                "-o",
                outputExecutablePath,
                resolver.ToolResource(tool, "crt1.o"),
                resolver.ToolResource(tool, "crti.o"),
                resolver.ToolResource(tool, "crtbegin.o"),
                $"-L{resolver.ToolResourcesFolder(tool)}",
                targetFiles,
                "-lgcc",
                "--as-needed",
                "-lgcc_s",
                "--no-as-needed",
                "-lc",
                "-lgcc",
                "--as-needed",
                "-lgcc_s",
                "--no-as-needed",
                resolver.ToolResource(tool, "crtend.o"),
                resolver.ToolResource(tool, "crtn.o")
            };

            // Return the resulting arguments.
            return args.ToArray();
        }

        protected string[] LinkMacOS()
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }

        protected string[] BuildWindows(string outputIrFilePath)
        {
            // Create the argument list.
            List<string> result = new List<string>
            {
                "-filetype=obj",
                "-mtriple=x86_64-pc-windows-msvc",
                outputIrFilePath
            };

            // Return the resulting argument list.
            return result.ToArray();
        }

        // TODO: Code is simply repeating Window's just changing target.
        protected string[] BuildLinux(string outputIrFilePath)
        {
            // Create the argument list.
            List<string> result = new List<string>
            {
                "-filetype=obj",
                "-mtriple=x86_64-pc-linux-gnu",
                outputIrFilePath
            };

            // Return the resulting argument list.
            return result.ToArray();
        }

        protected string[] BuildMacOS()
        {
            // TODO: Implement.
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
