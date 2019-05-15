using Ion.Core;
using IonCLI.Core;
using IonCLI.PackageManagement;

namespace IonCLI.Engines
{
    public struct EngineContext
    {
        public Options Options { get; set; }

        public Project Project { get; set; }

        public Package Package { get; set; }
    }
}
