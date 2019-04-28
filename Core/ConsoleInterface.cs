using System;
using System.IO;
using System.Text;

namespace LlvmSharpLang.CLI.Core
{
    public class FileResult
    {
        public string Path { get; }

        public byte[] Content { get; }

        public int Length
        {
            get => this.Content.Length;
        }

        public FileResult(string path)
        {
            this.Path = path;
            this.Content = File.ReadAllBytes(this.Path);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(this.Content);
        }
    }

    public class ConsoleInterface
    {
        // TODO: Needs to be '.xl'.
        public const string extension = ".cs";

        public string path;

        public bool relative;

        public FileResult[] files;

        public ConsoleInterface(string path)
        {
            // The path is relative if it starts with './' or '../'.
            this.relative = path.StartsWith(".");

            this.path = this.relative ? // If it's relative.
                        Path.Combine(Directory.GetCurrentDirectory(), path) : // Combine the current directory and it.
                        path; // Else, use the path.
        }

        public ConsoleInterface Scan()
        {
            // Get the files, using AllDirectories search option to search subfolders.
            string[] filePaths = Directory.GetFiles(this.path, $"*{extension}", SearchOption.AllDirectories);

            this.files = new FileResult[filePaths.Length];

            // Make file results out of the paths.
            for (var i = 0; i < filePaths.Length; i++)
            {
                this.files[i] = new FileResult(filePaths[i]);
            }

            return this;
        }
    }

}
