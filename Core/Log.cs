using System;

namespace IonCLI.Core
{
    public static class Log
    {
        public static bool VerboseMode { get; set; } = false;

        /// <summary>
        /// Prints out an error message onto the console
        /// and terminates program execution with a faulty
        /// exit code.
        /// </summary>
        public static void Error(string message)
        {
            // Print the message.
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Error: {message}");
            Console.ResetColor();

            // Exit program.
            Environment.Exit(1);
        }

        public static void Verbose(string message)
        {
            // Print only if verbose mode is active.
            if (Log.VerboseMode)
            {
                Console.WriteLine($"Verbose: message");
            }
        }
    }
}
