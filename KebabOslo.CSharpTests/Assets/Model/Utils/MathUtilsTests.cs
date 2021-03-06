﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class MathUtilsTests
    {
        [TestMethod()]
        public void DistanceTest()
        {
            Assert.AreEqual(0, MathUtils.Distance(0, 0, 0, 0)); // Same spot
            Assert.AreEqual(3, MathUtils.Distance(0, 0, 0, 3)); // A Line
            Assert.AreEqual(3, MathUtils.Distance(0, 0, 3, 0)); // A Line
        }

        [TestMethod()]
        public void LinearConversionInvertedTest()
        {
            Assert.AreEqual(1, MathUtils.LinearConversionInverted(0.0, 1.0, 1));
            Assert.AreEqual(1, MathUtils.LinearConversionInverted(0.0, 2.0, 1));
            Assert.AreEqual(50, MathUtils.LinearConversionInverted(5.0, 10.0, 100));
            Assert.AreEqual(0, MathUtils.LinearConversionInverted(1.0, 1.0, 100));
            Assert.AreEqual(25, MathUtils.LinearConversionInverted(3.0, 4.0, 100));
        }
    }
}