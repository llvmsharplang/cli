using System;
using System.IO;

namespace LlvmSharpLang.CLI.Core
{
    public class FileResult
    {
        public string path { get; }

        public byte[] content { get; }

        public int Length { get => this.content.Length; }

        public FileResult(string path)
        {
            this.path = path;
            this.content = File.ReadAllBytes(this.path);
        }

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(this.content);
        }
    }

    public class ConsoleInterface
    {
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
