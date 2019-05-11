using System.Text.RegularExpressions;

namespace IonCLI.Integrity
{
    public struct ToolDefinition
    {
        /// <summary>
        /// The filename of the tool, along with its
        /// extension.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The used to invoke the tool and verify its
        /// integrity by comparing its output.
        /// </summary>
        public string Invoker { get; set; }

        /// <summary>
        /// The expected regex-matched string to test when invoking
        /// the tool's corresponding command.
        /// </summary>
        public string ExpectedMatch { get; set; }

        /// <summary>
        /// The pattern to test against the output of the command
        /// invocation.
        /// </summary>
        public Regex MatchPattern { get; set; }
    }
}
