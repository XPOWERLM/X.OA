using System;
using NUnit.Framework;
using X.OA.Common.Utility;

namespace X.OA.UnitTest.UtilityTests
{
    [TestFixture]
    public class StringUtilityTests
    {
        [Test]
        public void IsNullOrEmpty_NoParameter_Throws()
        {
            Exception ex = Assert.Catch<ArgumentException>(() => StringUtility.IsNullOrEmpty());

            StringAssert.Contains("Nothing", ex.Message);
        }
    }
}
