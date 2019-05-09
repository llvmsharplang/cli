namespace Ion.CLI.Core
{
    internal static class VerifierConstants
    {
        /// <summary>
        /// The folder path on which the required LLVM tools
        /// are stored in. Windows only.
        /// </summary>
        public const string ToolsPath = "llvm-tools";

        /// <summary>
        /// The LLVM tools filenames expected to be within
        /// the LLVM tools folder.
        /// </summary>
        public static string[] Tools = new string[]
        {
            "lli.exe",
            "llc.exe"
        };
    }
}
