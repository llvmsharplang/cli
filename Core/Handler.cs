using System;
using System.Collections.Generic;
using System.IO;
using LLVMSharp;
using LlvmSharpLang.Abstraction;
using LlvmSharpLang.Linking;
using LlvmSharpLang.Parsing;
using LlvmSharpLang.SyntaxAnalysis;

namespace LlvmSharpLang.CLI.Core
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

            // Process files.
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file} ...");

                // Retrieve file contents.
                string content = File.ReadAllText(file);

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
                    Type = TokenType.KeywordFunction
                });

                // Create the driver.
                Driver driver = new Driver(stream);

                // Invoke the driver.
                driver.Next();

                // Emit the result.
                this.Emit(driver.Module);
            }

            // TODO: At this point, something is changing the console color to yellow, probably core lib.
            Console.WriteLine($"Processed {files.Length} file(s).");
        }

        public void Emit(Abstraction.Module module)
        {
            // Print the resulting IR code to the output target if applicable.
            if (this.options.PrintIr)
            {
                string error;

                LLVM.PrintModuleToFile(module.Source, Path.Join(this.options.Output, "program"), out error);
            }
            // Otherwise, emit compiled result.
            else
            {
                // TODO
            }
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
