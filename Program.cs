using System.IO;
using System;
using LlvmSharpLang.CLI.Core;
using LlvmSharpLang;
using CommandLine;

namespace LlvmSharpLang.CLI
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed<Options>((options) => RunOptionsAndReturnExitCode(options))
                .WithNotParsed<Options>((errors) => HandleParseErrors(errors));
        }
    }
}
