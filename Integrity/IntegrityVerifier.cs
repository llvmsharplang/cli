using System;
using System.IO;
using System.Runtime.InteropServices;
using IonCLI.Core;

namespace IonCLI.Integrity
{
    public class IntegrityVerifier
    {
        protected bool isWindowsOs;

        protected readonly string root;

        protected readonly Options options;

        public IntegrityVerifier(Options options, string root)
        {
            this.options = options;
            this.root = root;
            this.isWindowsOs = false;
        }

        /// <summary>
        /// Begin the verification process.
        /// </summary>
        public void Invoke()
        {
            // Perform OS check.
            this.PerformOsCheck();

            // Ensure tools, if applicable.
            this.TestTools();
        }

        /// <summary>
        /// Performs a check on the current OS platform
        /// to ensure everything is as expected.
        /// </summary>
        public void PerformOsCheck()
        {
            // MacOS is not currently supported.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Log.Error("MacOS X is not currently supported.");
            }
            // Set the Windows flag if current OS is Windows.
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.isWindowsOs = true;
            }
        }

        /// <summary>
        /// If applicable, ensure tools exist and are
        /// in an expected state.
        /// </summary>
        public void TestTools()
        {
            // Ensure tools have been downloaded on Windows.
            if (this.isWindowsOs)
            {
                // Tools directory must exist on Windows.
                if (!Directory.Exists(this.options.ToolsPath))
                {
                    Log.Error("Tools directory does not exist. You may have a corrupt installation. Try running the installation script again.");
                }

                // Ensure all tools exist.
                foreach (ToolDefinition tool in VerifierConstants.Tools)
                {
                    // Create the path for the tool.
                    string path = Path.Combine(this.options.ToolsPath, tool.FileName);

                    // Ensure tool exists.
                    if (!File.Exists(path))
                    {
                        Log.Error($"Required tool executable '{path}' is missing. You may have a corrupt installation.");
                    }
                }

                // Ensure tools execute and have correct versions.
                // TODO

                // Do not continue at this point.
                return;
            }
        }
    }
}
