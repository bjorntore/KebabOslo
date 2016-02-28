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

    }
}