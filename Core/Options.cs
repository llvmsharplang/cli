using System.Collections.Generic;
using CommandLine;

namespace IonCLI.Core
{
    internal class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('e', "exclude", Required = false, HelpText = "Exclude certain directories from being processed.")]
        public IEnumerable<string> Exclude { get; set; }

        [Option('o', "output", Required = false, HelpText = "The output directory which the program will be emitted onto.", Default = "ion.bin")]
        public string Output { get; set; }

        [Option('r', "root", Required = false, HelpText = "The root directory to start the scanning process from.")]
        public string Root { get; set; }

        [Option('b', "bitcode", Required = false, HelpText = "Print out the LLVM Bitcode code instead of LLVM IR.")]
        public bool Bitcode { get; set; }

        [Option('s', "silent", Required = false, HelpText = "Do not output any messages.")]

        public bool Silent { get; set; }

        [Option('i', "no-integrity", Required = false, HelpText = "Skip integrity check.")]
        public bool NoIntegrity { get; set; }

        [Option('d', "debug", Required = false, HelpText = "Use debugging mode.")]
        public bool DebugMode { get; set; }
    }
}
