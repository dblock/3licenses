using System;
using NUnit.Framework;

namespace AppSecInc.LicensesCollector
{
    [TestFixture]
    class UnitTests
    {
        [Test]
        public void TestLicensesCollector()
        {
            CollectLicenses collector = new CollectLicenses();

            collector.Src = @"c:\scanengine\trunk\externals";
            collector.ToDir = @"..\..\..\export\licenses";
            collector.MaxDepth = 4;
            collector.Execute();
        }
    }
}
