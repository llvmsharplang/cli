using System.IO;
using System;
using IonCLI.Core;
using Ion;
using CommandLine;
using System.Collections.Generic;
using Ion.Linking;
using Ion.SyntaxAnalysis;
using Ion.Parsing;
using LLVMSharp;
using IonCLI.Integrity;

namespace IonCLI
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default
                .ParseArguments<Options>(args)
                .WithNotParsed<Options>(Program.HandleParseErrors)

                // Process request.
                .WithParsed<Options>((options) =>
                {
                    // Set verbose mode.
                    Log.VerboseMode = options.Verbose;

                    // Set external output mode.
                    Log.OutputMode = options.ExternalOutput;

                    // Inform the user that verbose mode is active.
                    Log.Verbose("Using verbose mode.");

                    // Inform the user that external output mode is active, if applicable.
                    if (options.ExternalOutput)
                    {
                        Log.Verbose("Using external output mode");
                    }

                    // Override specific options if debug mode is active.
                    if (options.DebugMode)
                    {
                        // Inform the user that debug mode is active.
                        Log.Verbose("Using debug mode.");

                        // Retrieve the current, working directory.
                        string currentDirectory = Directory.GetCurrentDirectory();

                        // Override the tools path by providing the current working directory as the root.
                        options.ToolsPath = Path.Combine(currentDirectory, options.ToolsPath);
                    }

                    // Check integrity if applicable.
                    if (!options.NoIntegrity)
                    {
                        // Create a new verifier instance with the base directory.
                        IntegrityVerifier verifier = new IntegrityVerifier(options, AppContext.BaseDirectory);

                        // Invoke the verifier.
                        verifier.Invoke();
                    }
                    // Inform the user that integrity check is disabled.
                    else
                    {
                        Log.Verbose("Integrity check is disabled.");
                    }

                    // Create a new handler instance.
                    Handler handler = new Handler(options);

                    // Invoke the handler.
                    handler.Process();
                });
        }

        private static void HandleParseErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("There were errors processing the request.");

            // Display all errors.
            foreach (Error error in errors)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}
