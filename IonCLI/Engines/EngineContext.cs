using Ion.Core;
using IonCLI.Core;

namespace IonCLI.Engines
{
    public struct EngineContext
    {
        public Options Options { get; set; }

        public Project Project { get; set; }
    }
}
