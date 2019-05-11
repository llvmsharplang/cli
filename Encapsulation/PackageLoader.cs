using System;
using System.IO;
using System.Xml.Serialization;

namespace IonCLI.Encapsulation
{
    internal class PackageLoader
    {
        /// <summary>
        /// Whether the package manifest file exists
        /// inside the provided root directory path.
        /// </summary>
        public bool DoesManifestExist
        {
            get
            {
                // Retrieve the package manifest path.
                string path = this.ManifestFilePath;

                // Determine whether the manifest file exists.
                return File.Exists(path);
            }
        }

        /// <summary>
        /// The manifest path located under the provided root path.
        /// </summary>
        public string ManifestFilePath => this.ResolvePath(PackageConstants.ManifestFilename);

        protected readonly string root;

        public PackageLoader(string root)
        {
            this.root = root;
        }

        /// <summary>
        /// Resolve a path located under the provided root
        /// directory path.
        /// </summary>
        protected string ResolvePath(string path)
        {
            // Combine root path with the provided path.
            return Path.Combine(this.root, path);
        }

        /// <summary>
        /// Attempts to load the content of the manifest file
        /// and serialize it onto a Package class instance.
        /// </summary>
        public Package ReadPackage()
        {
            // Ensure manifest file exists.
            if (!this.DoesManifestExist)
            {
                throw new InvalidOperationException("Cannot read package manifest because it does not exist");
            }

            // Open a file stream to the manifest.
            FileStream manifestStream = new FileStream(this.ManifestFilePath, FileMode.Open);

            // Create the XML serializer instance.
            XmlSerializer serializer = new XmlSerializer(typeof(Package));

            // Invoke serializer.
            Package package = (Package)serializer.Deserialize(manifestStream);

            // Return the serialized package class instance.
            return package;
        }
    }
}
