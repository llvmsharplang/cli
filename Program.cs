using System.IO;
using System;
using LlvmSharpLang.CLI.Core;
using LlvmSharpLang;
using CommandLine;
using System.Collections.Generic;
using LlvmSharpLang.Linking;
using LlvmSharpLang.SyntaxAnalysis;
using LlvmSharpLang.Parsing;

namespace LlvmSharpLang.CLI
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed<Options>(Program.ProcessOptions)
                .WithNotParsed<Options>(Program.HandleParseErrors);
        }

        private static void HandleParseErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("There were errors processing the request.\n");

            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
        }

        private static void ProcessOptions(Options options)
        {
            Console.WriteLine("Discovering files ...");

            // Set the root directory.
            string root = Directory.GetCurrentDirectory();

            // Use specified root directory.
            if (!String.IsNullOrEmpty(options.Root))
            {
                // Ensure provided root directory exists.
                if (!Directory.Exists(options.Root))
                {
                    Program.Fatal("The specified root directory path does not exist");
                }

                // Use provided root directory path.
                root = options.Root;
            }

            // Ensure output directory exists, otherwise create it.
            if (!Directory.Exists(options.Output))
            {
                Console.WriteLine("Creating output directory ...");
                Directory.CreateDirectory(options.Output);
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

                // Create the driver.
                Driver driver = new Driver(stream);

                // Invoke the driver.
                driver.Next();

                // Print out the resulting IR code (temporarily).
                Console.WriteLine($"Output:\n{driver.Module.ToString()}");
            }

            // TODO: At this point, something is changing the console color to yellow, probably core lib.
            Console.WriteLine($"Processed {files.Length} file(s).");
        }

        /// <summary>
        /// Display an error message and terminate the
        /// program.
        /// </summary>
        private static void Fatal(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fatal: {message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}
