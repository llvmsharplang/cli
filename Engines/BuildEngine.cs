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

            // Create a new tool invoker instace.
            ToolInvoker toolInvoker = new ToolInvoker(this.context.Options);

            // Invoke the LLC tool to compile to object code.
            toolInvoker.Invoke(ToolType.LLC, new string[]
            {
                
            });
        }
    }
}
