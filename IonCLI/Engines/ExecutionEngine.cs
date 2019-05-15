using System;

namespace IonCLI.Engines
{
    public class ExecutionEngine : OperationEngine
    {
        public ExecutionEngine(EngineContext context) : base(context)
        {
            this.Dependencies = new OperationEngine[]
            {
                new BuildEngine(context)
            };
        }

        public override void Invoke()
        {
            // Prepare engine.
            this.Prepare();

            // TODO: Implement.
            throw new NotImplementedException();
        }
    }
}
