using System.Collections.Generic;

namespace Ion.CLI.Encapsulation
{
    public class Package
    {
        /// <summary>
        /// The uniquely identifying package name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The optional description string of the package.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The version string of the package.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The compile version iteration handled
        /// by the compiler, incremented every compilation
        /// step.
        /// </summary>
        public long Iteration { get; set; }

        /// <summary>
        /// The author(s)' names or contact information.
        /// </summary>
        public IEnumerable<string> Authors { get; set; }

        /// <summary>
        /// Packages needed or used within this application.
        /// </summary>
        public IEnumerable<Package> Dependencies { get; set; }
    }
}
