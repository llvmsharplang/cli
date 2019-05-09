using System;

namespace Ion.CLI.Core
{
    internal static class FatalError
    {
        /// <summary>
        /// Prints out an error message onto the console
        /// and terminates program execution with a faulty
        /// exit code.
        /// </summary>
        public static void Print(string message)
        {
            // Print the message.
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Fatal Error: {message}");
            Console.ResetColor();

            // Exit program.
            Environment.Exit(1);
        }
    }
}
