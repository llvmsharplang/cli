namespace IonCLI.Encapsulation
{
    public class Dependency
    {
        /// <summary>
        /// The uniquely identifying dependency name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version string of the dependency.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Whether this is an optional dependency.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Whether this is a development-only dependency.
        /// </summary>
        public bool Development { get; set; }

        /// <summary>
        /// The source location of this dependency.
        /// </summary>
        public DependencySource Source { get; set; }
    }
}
