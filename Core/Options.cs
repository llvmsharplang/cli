using System.Collections.Generic;
using CommandLine;

namespace Ion.CLI.Core
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('e', "exclude", Required = false, HelpText = "Exclude certain directories from being processed.")]
        public IEnumerable<string> Exclude { get; set; }

        [Option('o', "output", Required = false, HelpText = "The output directory which the program will be emitted onto.", Default = "l.bin")]
        public string Output { get; set; }

        [Option('r', "root", Required = false, HelpText = "The root directory to start the scanning process from.")]
        public string Root { get; set; }

        [Option('i', "ir", Required = false, HelpText = "Print out the emitted IR code instead of the compiled result.")]
        public bool PrintIr { get; set; }
    }
}

