using System.IO;
using System;
using Ion.CLI.Core;
using Ion;
using CommandLine;
using System.Collections.Generic;
using Ion.Linking;
using Ion.SyntaxAnalysis;
using Ion.Parsing;
using LLVMSharp;

namespace Ion.CLI
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
                    Handler handler = new Handler(options);

                    handler.Process();
                });
        }

        private static void HandleParseErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("There were errors processing the request.\n");

            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}
