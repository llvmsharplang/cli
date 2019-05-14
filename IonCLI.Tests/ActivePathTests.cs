using System.IO;
using IonCLI.Core;
using NUnit.Framework;

namespace IonCLI.Tests
{
    [TestFixture]
    internal sealed class ActivePathTests
    {
        public const string root = "root";

        [Test]
        [TestCase("test", "root/test")]
        [TestCase("test.ext", "root/test.ext")]
        public void Resolve(string input, string expected)
        {
            // Create the active path instance.
            PathResolver activePath = new PathResolver(ActivePathTests.root);

            // Prepare the input path.
            input = Util.CleanPathSeparators(input);

            // Resolve the input path.
            string actual = activePath.Resolve(input);

            // Compare result.
            Assert.AreEqual(expected, actual);
        }
    }
}
