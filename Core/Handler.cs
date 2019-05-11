using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LLVMSharp;
using Ion.Abstraction;
using Ion.Linking;
using Ion.Parsing;
using Ion.SyntaxAnalysis;
using IonCLI.Encapsulation;

namespace IonCLI.Core
{
    internal class Handler
    {
        protected readonly Options options;

        protected readonly Processor processor;

        public Handler(Options options)
        {
            this.options = options;

            // Create the processor instance.
            this.processor = new Processor(this);
        }

        protected void Print(string message)
        {
            // Only output if silent mode is not enabled.
            if (!this.options.Silent)
            {
                Console.WriteLine(message);
            }
        }

        public void Process()
        {
            // Set the root directory.
            string root = Directory.GetCurrentDirectory();

            // Use specified root directory.
            if (!String.IsNullOrEmpty(this.options.Root))
            {
                // Ensure provided root directory exists.
                if (!Directory.Exists(options.Root))
                {
                    this.Fatal("The specified root directory path does not exist");
                }

                // Use provided root directory path.
                root = Path.GetFullPath(this.options.Root);
            }

            // Create a new package loader instance.
            PackageLoader packageLoader = new PackageLoader(root);

            // Inform the user of the final root directory.
            Console.WriteLine($"Using root directory: {root}");

            // Ensure package manifest exists.
            if (!packageLoader.DoesManifestExist)
            {
                this.Fatal("Package manifest file does not exist.");
            }

            // Load the package manifest.
            Package package = packageLoader.ReadPackage();

            // Process package options if applicable.
            if (package.Options != null)
            {
                // Use package's root path option if applicable.
                if (package.Options.SourceRoot != null)
                {
                    // Create the new root path.
                    string newRoot = Path.GetFullPath(package.Options.SourceRoot);

                    // Ensure directory path exists.
                    if (!Directory.Exists(newRoot))
                    {
                        this.Fatal("Provided source root directory path in package manifest does not exist");
                    }

                    // Override root path.
                    root = Path.GetFullPath(package.Options.SourceRoot);

                    // Inform the user of the action taken.
                    Console.WriteLine($"Using source root directory from package manifest: {root}");
                }
            }

            // Process scanner.
            this.ProcessScanner(root);
        }

        public string Emit(Ion.Abstraction.Module module)
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
                    this.Fatal($"There was an error writing LLVM bitcode to '{targetPath}'.");
                }
            }

            // Read and obtain emitted data.
            result = File.ReadAllText(targetPath);

            // Return resutl.
            return result;
        }

        /// <summary>
        /// Display an error message and terminate the
        /// program.
        /// </summary>
        public void Fatal(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            this.Print($"Fatal: {message}");
            Console.ResetColor();
            Environment.Exit(1);
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
                this.Print("No matching files discovered.");
                Environment.Exit(0);
            }

            // Ensure output directory exists, otherwise create it.
            if (!Directory.Exists(this.options.Output))
            {
                this.Print("Creating output directory ...");
                Directory.CreateDirectory(this.options.Output);
            }

            // Process files.
            foreach (string file in files)
            {
                // Inform the user that of the file being processed.
                this.Print($"Processing {file} ...");

                // Process file and obtain resulting output.
                string result = this.processor.ProcessFile(file);
            }

            // TODO: At this point, something is changing the console color to yellow, probably core lib.
            this.Print($"Processed {files.Length} file(s).");
        }
    }
}
