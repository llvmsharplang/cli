using System.Collections.Generic;
using System.Text.RegularExpressions;
using IonCLI.Core;

namespace IonCLI.Integrity
{
    public static class VerifierConstants
    {
        /// <summary>
        /// The default folder path on which the required tools
        /// are stored in. Windows only.
        /// </summary>
        public const string DefaultToolsPath = "tools";

        /// <summary>
        /// The default version string expected to be captured during
        /// the invocation of the tools.
        /// </summary>
        public const string DefaultLlvmVersion = "5.0.0";

        /// <summary>
        /// Matches a generic version string from the version command
        /// invocation output.
        /// </summary>
        public static Regex GenericVersionPattern = new Regex(@"[0-9]\.[0-9]\.[0-9]");

        /// <summary>
        /// The tool filenames expected to be within
        /// the tools folder.
        /// </summary>
        public static Dictionary<ToolType, ToolDefinition> Tools = new Dictionary<ToolType, ToolDefinition>
        {
            // LLI (LLVM direct executioner).
            {ToolType.LLI, new ToolDefinition
            {
                FileName = "lli.exe",
                Invoker = "lli --version",
                MatchPattern = VerifierConstants.GenericVersionPattern,
                ExpectedMatch = VerifierConstants.DefaultLlvmVersion
            }},

            // LLC (LLVM static compiler).
            {ToolType.LLC, new ToolDefinition
            {
                FileName = "llc.exe",
                Invoker = "llc --version",
                MatchPattern = VerifierConstants.GenericVersionPattern,
                ExpectedMatch = VerifierConstants.DefaultLlvmVersion
            }},

            // LLD (LLVM linker).
            {ToolType.LLD, new ToolDefinition
            {
                FileName = "lld.exe"
            }},

            // Link (Visual Studio's, Windows-only linker).
            {ToolType.Link, new ToolDefinition
            {
                FileName = "link.exe",
                Root = "link"
            }}
        };
    }
}
