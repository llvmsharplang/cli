using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LLVMSharp;
using Ion.Abstraction;
using Ion.Linking;
using Ion.Parsing;
using Ion.SyntaxAnalysis;

namespace Ion.CLI.Core
{
    public class Handler
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
                root = this.options.Root;
            }

            // Ensure output directory exists, otherwise create it.
            if (!Directory.Exists(this.options.Output))
            {
                this.Print("Creating output directory ...");
                Directory.CreateDirectory(this.options.Output);
            }

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

        public string Emit(Abstraction.Module module)
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
    }
}
