using System.Collections.Generic;
using CommandLine;

namespace LlvmSharpLang.CLI.Core
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('e', "exclude", Required = false, HelpText = "Exclude certain directories from being processed.")]
        public IEnumerable<string> Exclude { get; set; }

        [Option('o', "Output", Required = false, HelpText = "The output directory which the program will be emitted onto.")]
        public string Output { get; set; }
    }
}

