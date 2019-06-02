using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ion.Core;

namespace IonCLI.InterOperability
{
    public static class Platform
    {
        private static Dictionary<OSPlatform, string> platformIdMap = new Dictionary<OSPlatform, string>
        {
            {OSPlatform.Linux, PlatformId.Linux},
            {OSPlatform.OSX, PlatformId.MacOS}
        };

        public static string Id { get; private set; }

        static Platform()
        {
            Platform.Id = Platform.GetId();
        }

        public static string GetId()
        {
            // Retrieve the OS' architecture.
            Architecture architecture = RuntimeInformation.OSArchitecture;

            // First, detect Windows as a special case.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Determine and return the corresponding id depending on Windows' OS architecture.
                return architecture == Architecture.X64 ? PlatformId.Windows64 : PlatformId.Windows32;
            }
            // Platform is not Unix ARM-like.
            else if (architecture != Architecture.Arm && architecture != Architecture.Arm64)
            {
                // Iterate through the platform id map.
                foreach ((OSPlatform platform, string platformId) in Platform.platformIdMap)
                {
                    // Determine if platform matches.
                    if (RuntimeInformation.IsOSPlatform(platform))
                    {
                        // Platform id was successfully matched.
                        return platformId;
                    }
                }
            }

            // Otherwise, default to ARMv7.
            return PlatformId.ARMv7;
        }
    }
}
