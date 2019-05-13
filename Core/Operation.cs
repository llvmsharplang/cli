using System.Collections.Generic;

namespace IonCLI.Core
{
    internal enum OperationType
    {
        Unknown,

        Build,

        Run,

        Init
    }

    internal static class Operation
    {
        private static readonly Dictionary<string, OperationType> valueMap = new Dictionary<string, OperationType>
        {
            {"build", OperationType.Build},
            {"run", OperationType.Run},
            {"init", OperationType.Init}
        };

        public static OperationType Resolve(string value)
        {
            // Ensure the string value is mapped.
            if (!Operation.valueMap.ContainsKey(value))
            {
                // Otherwise, return an unknown operation.
                return OperationType.Unknown;
            }

            // The operation string value is registered, resolve it.
            return Operation.valueMap[value];
        }
    }
}
