using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void RandomIntTest()
        {
            Assert.AreEqual(0, Utils.RandomInt(0, 0));
            Assert.AreEqual(1, Utils.RandomInt(1, 1));
        }

        [TestMethod()]
        public void RandomIntDoubleTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int roll = Utils.RandomInt(0.0, 3.0);
                Assert.IsTrue(roll >= 0);
                Assert.IsTrue(roll <= 3);
                Console.WriteLine(roll);
            }
        }
    }
}