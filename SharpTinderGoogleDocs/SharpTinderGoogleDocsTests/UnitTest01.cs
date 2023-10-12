using SharpConfigProg.Preparer;
using SharpConfigProg.Service;

namespace SharpTinderComplexTests
{
    [TestClass]
    public class UnitTest01 : UnitTest01Base
    {
        [TestMethod]
        public void Phase_01_PreparePaths()
        {
            configService.Prepare(typeof(IPreparer.IWinder));
        }
    }
}