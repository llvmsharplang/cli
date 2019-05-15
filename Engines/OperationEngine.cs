using Ion.Core;

namespace IonCLI.Engines
{
    public abstract class OperationEngine
    {
        public virtual OperationEngine[] Dependencies { get; }

        public abstract void Invoke(Project project);

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
