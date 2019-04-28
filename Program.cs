using System.IO;
using System;
using LlvmSharpLang.CLI.Core;
using LlvmSharpLang;
using CommandLine;
using System.Collections.Generic;
using LlvmSharpLang.Linking;
using LlvmSharpLang.SyntaxAnalysis;
using LlvmSharpLang.Parsing;
using LLVMSharp;

namespace LlvmSharpLang.CLI
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
