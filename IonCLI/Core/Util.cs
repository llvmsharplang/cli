using System.IO;
using System.Runtime.InteropServices;

namespace IonCLI.Core
{
    public static class Util
    {
        public static bool IsWindowsOS => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static string CleanPathSeparators(string input)
        {
            // Interchange and replace path separator characters.
            input = input.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            input = input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            // Return the resulting input.
            return input;
        }
    }
}
