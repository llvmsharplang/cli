namespace IonCLI.Core
{
    public enum ToolType
    {
        /// <summary>
        /// Used to directly execute IR code
        /// without a compilation step.
        /// </summary>
        LLI,

        /// <summary>
        /// The LLVM static compiler tool, used to compile
        /// IR code to either object code or assembly code as targets.
        /// </summary>
        LLC,

        /// <summary>
        /// The LLVM linker tool, links multiple object files
        /// together into an executable runnable in the local
        /// machine.
        /// </summary>
        WindowsLLD,

        UnixLikeLLD,

        MacOsLD,

        /// <summary>
        /// Microsoft's Visual Studio's linker tool. Windows-only.
        /// </summary>
        Link
    }
}
