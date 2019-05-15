using System.Collections.Generic;
using IonCLI.Core;

namespace IonCLI.Engines
{
    public class BuildEngine : OperationEngine
    {
        public BuildEngine(EngineContext context) : base(context)
        {
            //
        }

        public override void Invoke()
        {
            // Prepare engine.
            this.Prepare();

            // Emit the project.
            Dictionary<string, string> modules = this.context.Project.Emit();

            System.Console.WriteLine("Pass");

            // Loop through all the emitted results.
            foreach ((string fileName, string output) in modules)
            {
                System.Console.WriteLine($"Filename: {fileName}");

                // Create a new tool invoker instace.
                ToolInvoker toolInvoker = new ToolInvoker(this.context.Options);

                // Invoke the LLC tool to compile to object code.
                toolInvoker.Invoke(ToolType.LLC, new string[]
                {
                    "-filetype obj",
                    fileName
                });
            }
        }
    }
}
