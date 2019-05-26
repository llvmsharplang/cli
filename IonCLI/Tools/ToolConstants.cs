using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ion.Core;
using IonCLI.Core;
using IonCLI.InterOperability;

namespace IonCLI.Tools
{
    public static class ToolConstants
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
        /// The tool filenames expected to be within the tools
        /// folder.
        /// </summary>
        public static Dictionary<ToolType, ToolDefinition> Tools = new Dictionary<ToolType, ToolDefinition>
        {
            // LLI (LLVM IR direct executioner).
            {ToolType.LLI, new ToolDefinition
            {
                FileName = "lli",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.Ubuntu14,
                    PlatformId.Ubuntu16,
                    PlatformId.Debian8,
                    PlatformId.ARMv7,
                    PlatformId.MacOS
                }
            }},

            // LLC (LLVM static compiler).
            {ToolType.LLC, new ToolDefinition
            {
                FileName = "llc",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.Ubuntu14,
                    PlatformId.Ubuntu16,
                    PlatformId.Debian8,
                    PlatformId.ARMv7,
                    PlatformId.MacOS
                }
            }},

            // LLD (LLVM linker).
            {ToolType.LLD, new ToolDefinition
            {
                FileName = "lld",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.Ubuntu14,
                    PlatformId.Ubuntu16,
                    PlatformId.Debian8,
                    PlatformId.ARMv7,
                    PlatformId.MacOS
                }
            }},

            // Link (Visual Studio's, Windows-only linker).
            {ToolType.Link, new ToolDefinition
            {
                FileName = "link",
                SubPath = "link",
                Platforms = new string[] {PlatformId.Windows64}
            }}
        };
    }
}
