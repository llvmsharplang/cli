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

        public const string ResourcesFolder = "resources";

        public static Dictionary<ToolType, ToolDefinition> Tools = new Dictionary<ToolType, ToolDefinition>
        {
            // LLI (LLVM IR direct executioner).
            {ToolType.Lli, new ToolDefinition
            {
                FileName = "lli",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.ARMv7,
                    PlatformId.Linux,
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
                    PlatformId.ARMv7,
                    PlatformId.Linux,
                    PlatformId.MacOS
                }
            }},

            // LLD (Generic linker driver).
            {ToolType.GenericLld, new ToolDefinition
            {
                FileName = "lld",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.Linux
                }
            }},

            // LD.LLD (Linux linker).
            {ToolType.LinuxLdLld, new ToolDefinition
            {
                FileName = "ld.lld",

                Platforms = new string[] {
                    PlatformId.Windows64,
                    PlatformId.Windows32,
                    PlatformId.ARMv7,
                    PlatformId.Linux,
                    PlatformId.MacOS
                }
            }},

            // Link (Visual Studio's, Windows-target linker).
            {ToolType.WindowsLink, new ToolDefinition
            {
                FileName = "link",
                SubPath = "link",
                Platforms = new string[] {PlatformId.Windows64}
            }}
        };
    }
}
