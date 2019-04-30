namespace Ion.CLI.Encapsulation
{
    public enum DependencySourceType
    {
        /// <summary>
        /// A project hosted on GitHub.
        /// </summary>
        GitHub,

        /// <summary>
        /// A project hosted on a Git URL.
        /// </summary>
        Git,

        /// <summary>
        /// A project hosted locally in the file system.
        /// </summary>
        Local
    }

    public class DependencySource
    {
        public DependencySourceType Type { get; set; }

        public string URL { get; set; }
    }
}
