using System.IO;
using System;
using LlvmSharpLang.CLI.Core;

namespace LlvmSharpLang.CLI
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            ConsoleInterface cli = new ConsoleInterface(args.Length > 0 ? args[0] : Directory.GetCurrentDirectory());

            cli.Scan();
            Console.WriteLine($"Paths: ({cli.files.Length})");

            for (var i = 0; i < cli.files.Length; i++)
            {
                Console.WriteLine($"Path {i + 1}: {cli.files[i].Path} | {cli.files[i].Length} bytes");
            }
        }
    }
}
