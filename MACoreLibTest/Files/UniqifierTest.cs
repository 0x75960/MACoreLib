using MACoreLib.Files;
using System;
using System.IO;
using Xunit;

namespace MACoreLibTest.Files
{
    public class UniqifierTest
    {
        [Fact]
        public void TestUniqify()
        {
            var d = Path.GetTempPath();

            var uniq = new Uniqifier(d);

            foreach (var f in uniq.GetUniquedFiles())
            {
                Console.WriteLine(f);
            }

            Assert.True(true);

        }
    }
}
