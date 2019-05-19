using System;
using System.Text;

namespace IonCLI.Core
{
    public static class Log
    {
        public static bool VerboseMode { get; set; } = false;

        public static bool SilentMode { get; set; } = false;

        public static bool ExternalOutputMode { get; set; } = false;

        public static void Compose(string message, ConsoleColor? color = null)
        {
            // Return immediately if in silent mode.
            if (Log.SilentMode)
            {
                return;
            }
            // Set color beforehand if applicable.
            else if (color.HasValue)
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
        public static void Error(string message, int exitCode = 1)
        {
            // Print the message.
            Log.Compose($"Error: {message}", ConsoleColor.Red);

            // Exit program.
            Environment.Exit(exitCode);
        }

        public static void Warning(string message)
        {
            // Print the message without conditions.
            Log.Compose($"Warning: {message}", ConsoleColor.Yellow);
        }

        public static void ExternalOutput(string message, string sender)
        {
            if (Log.VerboseMode && Log.ExternalOutputMode)
            {
                // Extract the title string.
                string title = $"External output from '{sender}'";

                // Output the title.
                Log.Compose(title);

                // Create the ray buffer.
                string ray = "";

                // Create a new string builder to append to the ray string.
                StringBuilder rayBuilder = new StringBuilder();

                // Fill ray string.
                for (int i = 0; i < title.Length; i++)
                {
                    rayBuilder.Append("-");
                }

                // Apply ray builder.
                ray = rayBuilder.ToString();

                // Output the start ray string.
                Log.Compose(ray);

                // Write the message.
                Log.Compose(message, ConsoleColor.Cyan);

                // Output the end ray string.
                Log.Compose(ray);
            }
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
