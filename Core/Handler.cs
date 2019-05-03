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

        public Handler(Options options)
        {
            this.options = options;
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
                Console.WriteLine("Creating output directory ...");
                Directory.CreateDirectory(this.options.Output);
            }

            // Create the scanner.
            Scanner scanner = new Scanner(root);

            // Scan for files.
            string[] files = scanner.Scan();

            // No matching files.
            if (files.Length == 0)
            {
                Console.WriteLine("No matching files discovered.");
                Environment.Exit(0);
            }

            // Initialize the program output string builder.
            StringBuilder programOutput = new StringBuilder();

            // Process files.
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file} ...");

                // Process file and obtain resulting output.
                string result = this.ProcessFile(file);

                // Append result to the string builder.
                programOutput.Append(result);
            }

            // TODO: Path hard-coded.
            string finalProgramPath = "./l.bin/program.final";

            // Write final program.
            File.WriteAllText(finalProgramPath, programOutput.ToString());

            // TODO: At this point, something is changing the console color to yellow, probably core lib.
            Console.WriteLine($"Processed {files.Length} file(s).");
        }

        public string ProcessFile(string path)
        {
            // Retrieve file contents.
            string content = File.ReadAllText(path);

            // Create the lexer.
            Lexer lexer = new Lexer(content);

            // Tokenize contents.
            List<Token> tokens = lexer.Tokenize();

            // Create the token stream.
            TokenStream stream = new TokenStream(tokens.ToArray());

            // TODO
            // Temporarily insert token stream bounds.
            stream.Insert(0, new Token
            {
                Type = TokenType.Unknown
            });

            // Create the driver.
            Driver driver = new Driver(stream);

            // Invoke the driver.
            driver.Next();

            // Emit the result.
            string result = this.Emit(driver.Module);

            // Return the result.
            return result;
        }

        public string Emit(Abstraction.Module module)
        {
            // Create the resulting string.
            string result;

            // Create the full, target output path.
            string targetPath;

            // Print the resulting LLVM IR code to the output target if applicable.
            if (this.options.PrintIr)
            {
                // TODO: Make use of this.
                string error;

                // Create the target path.
                targetPath = Path.Join(this.options.Output, "program.ll");

                // TODO: Should not write to file/create file.
                // Emit IR to target path.
                LLVM.PrintModuleToFile(module.Source, targetPath, out error);
            }
            // Otherwise, emit LLVM bitcode result.
            else
            {
                // Create the target path.
                targetPath = Path.Join(this.options.Output, "program.bc");

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
            Console.WriteLine($"Fatal: {message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}
