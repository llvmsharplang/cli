using System;

namespace IonCLI.Core
{
    public static class Log
    {
        public static bool VerboseMode { get; set; } = false;

        public static bool SilentMode { get; set; } = false;

        public static void Compose(string message, ConsoleColor? color = null)
        {
            // Set color beforehand if applicable.
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }

            // Print the message.
            Console.WriteLine(message);

            // Reset color afterwards if applicable.
            if (color.HasValue)
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Prints out an error message onto the console
        /// and terminates program execution with a faulty
        /// exit code.
        /// </summary>
        public static void Error(string message)
        {
            // Print the message.
            Log.Compose($"Error: {message}", ConsoleColor.Red);

            // Exit program.
            Environment.Exit(1);
        }

        public static void Verbose(string message)
        {
            // Print only if verbose mode is active.
            if (Log.VerboseMode)
            {
                // Print the message.
                Log.Compose($"Verbose: {message}", ConsoleColor.DarkGray);
            }
        }

        public static void Success(string message)
        {
            // Print the message without conditions.
            Log.Compose(message, ConsoleColor.Green);
        }
    }
}
