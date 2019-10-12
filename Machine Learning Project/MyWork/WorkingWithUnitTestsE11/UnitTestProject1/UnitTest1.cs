using ClassLibraryE11;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testInstance = new Class1();

            Assert.IsTrue(testInstance.multiply(5, 2) == 10);
            Assert.IsTrue(testInstance.multiply(5, 2) == 34);
        }
    }
}
