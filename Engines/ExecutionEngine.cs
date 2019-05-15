using System;

namespace IonCLI.Engines
{
    public class ExecutionEngine : OperationEngine
    {
        public ExecutionEngine()
        {
            this.Dependencies = new OperationEngine[]
            {
                new BuildEngine()
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
