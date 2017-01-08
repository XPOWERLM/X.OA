using Microsoft.VisualStudio.TestTools.UnitTesting;
using X.OA.IDAL;
using X.OA.Model;

namespace X.OA.Factory.Tests
{
    [TestClass]
    public class DbSessionTests
    {
        [TestMethod]
        public void Set_WhenCalls_ReturnDAL()
        {
            string expectedType = "UserInfoDAL";
            IDbSession dbsession = DbSessionFactoty.CreateDbSession<UserInfo>();

            IBaseDAL<UserInfo> userinfoDAL = dbsession.Set<UserInfo>();
            string actualType = userinfoDAL.GetType().Name;

            Assert.AreEqual(expectedType, actualType);
        }
    }
}
