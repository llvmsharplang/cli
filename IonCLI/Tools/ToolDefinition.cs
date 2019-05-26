namespace IonCLI.Tools
{
    public struct ToolDefinition
    {
        /// <summary>
        /// The filename of the tool, along with its
        /// extension.
        /// </summary>
        public string FileName { get; set; }

        public string[] Platforms { get; set; }

        /// <summary>
        /// The sub directory on which the tool's executable
        /// is located, under the tools directory and its corresponding
        /// platform directory.
        /// </summary>
        public string SubPath { get; set; }
    }
}
