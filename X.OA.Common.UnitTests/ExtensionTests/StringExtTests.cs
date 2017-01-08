using System;
using NUnit.Framework;

namespace X.OA.UnitTest
{
    [TestFixture]
    public class StringExtTests
    {
        [TestCase("bad value")]
        [TestCase("2147483647000000000000")]
        public void ToInt32_InvalidValue_ReturnZero(string value)
        {
            // Arrange
            string invalidValue = value;

            // Action
            int result = invalidValue.ToInt32();

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ToInt32_GoodValue_ReturnItself()
        {
            string goodValue = "2016";

            int result = goodValue.ToInt32();

            Assert.AreEqual(2016, result);
        }

    }
}
