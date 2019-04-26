using System.IO;
using System;

namespace cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            CLI cli = new CLI(args.Length > 0 ? args[0] : Directory.GetCurrentDirectory());
            cli.Scan();
            Console.WriteLine($"Paths: ({cli.files.Length})");
            for (var i = 0; i < cli.files.Length; i++)
            {
                Console.WriteLine($"Path {i + 1}: {cli.files[i].path} | {cli.files[i].Length} bytes");
            }
        }
    }
}
