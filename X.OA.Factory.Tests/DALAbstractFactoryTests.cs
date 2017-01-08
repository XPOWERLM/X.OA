using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X.OA.Model;

namespace X.OA.Factory.Tests
{
    [TestClass]
    public class DALAbstractFactoryTests
    {
        [TestMethod]
        public void CreateInstance_WhenCalls_ReturnDALInstance()
        {
            string expectedType = "UserInfoDAL";

            object userinfoDAL = DALAbstractFactory.CreateInstance<UserInfo>();
            string actualType = userinfoDAL.GetType().Name;

            Assert.AreEqual(expectedType, actualType);
        }
    }
}
