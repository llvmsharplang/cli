using IonCLI.Core;
using NUnit.Framework;

namespace IonCLI.Tests
{
    [TestFixture]
    internal sealed class ActivePathTests
    {
        [Test]
        [TestCase("test", "root/test")]
        [TestCase("test.ext", "root/test.ext")]
        public void Resolve(string input, string expected)
        {
            // Create the root path.
            string root = "root";

            // Create the active path instance.
            ActivePath activePath = new ActivePath(root);

            // Resolve the input path.
            string actual = activePath.Resolve(input);

            // Compare result.
            Assert.AreEqual(expected, input);
        }
    }
}
