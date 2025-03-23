using Assert.Infrastructure.Utils;

namespace Assert.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetHash512()
        {
            string value = "Passw0rd";
            var result = UtilsMgr.GetHash512(value);
        }
    }
}