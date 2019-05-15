using System;
using Ion.Core;

namespace IonCLI.Engines
{
    public abstract class OperationEngine
    {
        public virtual OperationEngine[] Dependencies { get; protected set; }

        protected readonly EngineContext context;

        public OperationEngine(EngineContext context)
        {
            this.context = context;
            this.Dependencies = new OperationEngine[] { };

            // Ensure context values are not null.
            if (this.context.Options == null || this.context.Project == null)
            {
                throw new NullReferenceException("Unexpected property in context to be null");
            }
        }

        public abstract void Invoke();

        protected virtual void Prepare()
        {
            // Loop through all dependencies.
            foreach (OperationEngine engine in this.Dependencies)
            {
                // Invoke dependent engine.
                engine.Invoke();
            }
        }
    }
}
