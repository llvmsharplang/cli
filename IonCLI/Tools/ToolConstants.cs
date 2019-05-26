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
        /// are stored in.
        /// </summary>
        public const string DefaultToolsPath = "tools";

        public static Dictionary<ToolType, ToolDefinition> Tools = new Dictionary<ToolType, ToolDefinition>
        {
            // LLI (LLVM IR direct executioner).
            {ToolType.Lli, new ToolDefinition
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
            {ToolType.Llc, new ToolDefinition
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

            // LLD-LINK (Windows LLVM linker).
            {ToolType.WindowsLldLink, new ToolDefinition
            {
                FileName = "lld",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32
                }
            }},

            // LLD (Unix-like, non-MacOS LLVM linker).
            {ToolType.UnixLikeLld, new ToolDefinition
            {
                FileName = "ld.lld",

                Platforms = new string[] {
                    PlatformId.Ubuntu14,
                    PlatformId.Ubuntu16,
                    PlatformId.Debian8,
                    PlatformId.ARMv7
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
