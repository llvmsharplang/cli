using System.Text.RegularExpressions;

namespace Ion.CLI.Core
{
    public static class VerifierPatterns
    {
        /// <summary>
        /// Matches the version string from the LLI command's
        /// version output.
        /// </summary>
        public static Regex LliVersion = new Regex(@"[0-9]\.[0-9]\.[0-9]");
    }
}
