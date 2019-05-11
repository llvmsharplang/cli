namespace IonCLI.Encapsulation
{
    internal class Dependency : Package
    {
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
