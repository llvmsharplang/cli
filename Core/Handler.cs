using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LLVMSharp;
using Ion.Abstraction;
using Ion.Linking;
using Ion.Parsing;
using Ion.SyntaxAnalysis;
using IonCLI.PackageManagement;
using Ion.Core;

namespace IonCLI.Core
{
    internal class Handler
    {
        protected readonly Options options;

        protected readonly Processor processor;

        protected OperationType operation;

        public Handler(Options options)
        {
            this.options = options;

            // Create the processor instance.
            this.processor = new Processor(this);
        }

        public void Process()
        {
            // Retrieve operation value from options.
            string operationValue = this.options.Operation;

            // Inform the user of the requested operation if applicable.
            Log.Verbose($"Using operation: {operationValue}");

            // Resolve operation value.
            OperationType operation = Operation.Resolve(operationValue);

            // Ensure operation type is not unknown.
            if (operation == OperationType.Unknown)
            {
                Log.Error($"Unknown operation: '{operationValue}'");
            }

            // Inform the user that the requested operation is valid, if applicable.
            Log.Verbose("Requested operation is valid.");

            // Set the operation for future use.
            this.operation = operation;

            // Set the root directory.
            string root = Directory.GetCurrentDirectory();

            // Use specified root directory.
            if (!String.IsNullOrEmpty(this.options.Root))
            {
                // Ensure provided root directory exists.
                if (!Directory.Exists(options.Root))
                {
                    Log.Error("The specified root directory path does not exist");
                }

                // Use provided root directory path.
                root = Path.GetFullPath(this.options.Root);
            }

            // Inform the user of the final root directory.
            Log.Verbose($"Using root directory: {root}");

            // Create a new package loader instance.
            PackageLoader packageLoader = new PackageLoader(root);

            // Ensure package manifest exists.
            if (!packageLoader.DoesManifestExist)
            {
                Log.Error("Package manifest file does not exist.");
            }

            // Inform the user that the package manifest exists.
            Log.Verbose("Package manifest file exists.");

            // Load the package manifest.
            Package package = packageLoader.ReadPackage();

            // Inform the user that the package manifest was loaded.
            Log.Verbose("Package manifest file loaded.");

            // Process package options if applicable.
            if (package.Options != null)
            {
                // Use package's root path option if applicable.
                if (package.Options.SourceRoot != null)
                {
                    // Create the source directory path.
                    string sourcePath = Path.GetFullPath(package.Options.SourceRoot);

                    // Inform the user of the source directory path.
                    Log.Verbose($"Using source directory: {sourcePath}");

                    // Ensure directory path exists.
                    if (!Directory.Exists(sourcePath))
                    {
                        Log.Error("Provided source root directory path in package manifest does not exist");
                    }

                    // Inform the user that the source directory exists.
                    Log.Verbose("Source directory is valid.");

                    // Override root path.
                    root = Path.GetFullPath(package.Options.SourceRoot);

                    // Inform the user of the action taken.
                    Log.Verbose($"Using source root directory from package manifest: {root}");
                }
            }

            // Process scanner.
            this.ProcessScanner(root);
        }

        public void ProcessOperation(Driver driver)
        {
            // Ensure operation is valid.
            if (this.operation == OperationType.Unknown)
            {
                throw new InvalidOperationException("Unexpected operation to be unknown");
            }
            // Emit only if the operation is build.
            else if (this.operation == OperationType.Build)
            {
                // Pass along the module to the emit method.
                this.Emit(driver.Module);

                // Do not continue execution.
                return;
            }

            // At this point, operation must be run. Emit the module.
            this.Emit(driver.Module);

            // Create the tool invoker instance.
            ToolInvoker toolInvoker = new ToolInvoker();

            // TODO: Finish implementing.
            // Invoke the corresponding tool to execute the program.
            toolInvoker.Invoke(ToolType.LLI, new string[] { });
        }

        protected string Emit(Ion.Abstraction.Module module)
        {
            // Create the resulting string.
            string result;

            // Create the full, target output path.
            string targetPath;

            // Default to IR file extension.
            string extension = FileExtension.IR;

            // Print the resulting LLVM IR code to the output target if applicable.
            if (!this.options.Bitcode)
            {
                // TODO: Make use of this.
                string error;

                // Create the target path.
                targetPath = Path.Join(this.options.Output, $"{module.Name}.{extension}");

                // TODO: Should not write to file/create file.
                // Emit IR to target path.
                LLVM.PrintModuleToFile(module.Source, targetPath, out error);
            }
            // Otherwise, emit LLVM Bitcode result.
            else
            {
                // Set the extension to Bitcode.
                extension = FileExtension.Bitcode;

                // Create the target path.
                targetPath = Path.Join(this.options.Output, $"{module.Name}.{extension}");

                // TODO: Should not write to file/create file.
                // Write bitcode to target path.
                if (LLVM.WriteBitcodeToFile(module.Source, targetPath) != 0)
                {
                    Log.Error($"There was an error writing LLVM bitcode to '{targetPath}'.");
                }
            }

            // Read and obtain emitted data.
            result = File.ReadAllText(targetPath);

            // Return resutl.
            return result;
        }

        protected void ProcessScanner(string root)
        {
            // Create the scanner.
            Scanner scanner = new Scanner(root);

            // Scan for files.
            string[] files = scanner.Scan();

            // No matching files.
            if (files.Length == 0)
            {
                Log.Compose("No matching files discovered.");
                Environment.Exit(0);
            }

            // Ensure output directory exists, otherwise create it.
            if (!Directory.Exists(this.options.Output))
            {
                Log.Compose("Creating output directory ...");
                Directory.CreateDirectory(this.options.Output);
            }

            // Process files.
            foreach (string file in files)
            {
                // Inform the user that of the file being processed.
                Log.Compose($"Processing {file} ...");

                // Process file and obtain resulting output.
                string result = this.processor.ProcessFile(file);
            }

            // TODO: At this point, something is changing the console color to yellow, probably core lib.
            Log.Success($"Processed {files.Length} file(s).");
        }
    }
}
