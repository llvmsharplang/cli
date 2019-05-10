using System.Text.RegularExpressions;

namespace Ion.CLI.Integrity
{
    internal static class VerifierConstants
    {
        /// <summary>
        /// The folder path on which the required LLVM tools
        /// are stored in. Windows only.
        /// </summary>
        public const string ToolsPath = ".llvm-tools";

        /// <summary>
        /// The version string expected to be captured during
        /// the invocation of the tools.
        /// </summary>
        public const string LlvmVersion = "5.0.0";

        /// <summary>
        /// Matches a generic version string from the version command
        /// invocation output.
        /// </summary>
        public static Regex GenericVersionPattern = new Regex(@"[0-9]\.[0-9]\.[0-9]");

        /// <summary>
        /// The LLVM tools filenames expected to be within
        /// the LLVM tools folder.
        /// </summary>
        public static ToolDefinition[] Tools = new ToolDefinition[]
        {
            // LLI.
            new ToolDefinition
            {
                FileName = "lli.exe",
                Invoker = "lli --version",
                MatchPattern = VerifierConstants.GenericVersionPattern,
                ExpectedMatch = VerifierConstants.LlvmVersion
            },

            // LLC.
            new ToolDefinition
            {
                FileName = "llc.exe",
                Invoker = "llc --version",
                MatchPattern = VerifierConstants.GenericVersionPattern,
                ExpectedMatch = VerifierConstants.LlvmVersion
            }
        };
    }
}
