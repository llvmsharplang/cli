namespace IonCLI.InterOperability
{
    public static class PlatformId
    {
        public const string Windows64 = "win64";

        public const string Windows32 = "win32";

        public const string Debian8 = "debian8";

        public const string MacOS = "macOS";

        public const string Ubuntu16 = "ubuntu16.04";

        public const string Ubuntu14 = "ubuntu14.04";

        public const string ARMv7 = "armv7a";

        /// <summary>
        /// Represents a generic, Unix-like OS proxy,
        /// pointing to Ubuntu 16.04.
        /// </summary>
        public const string UnixLike = PlatformId.Ubuntu16;
    }
}