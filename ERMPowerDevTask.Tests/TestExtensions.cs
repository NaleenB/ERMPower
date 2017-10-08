using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ERMPowerDevTask.Helpers;

namespace ERMPowerDevTask.Tests
{
    [TestClass]
    public class TestExtensions
    {
        [TestMethod]
        public void TestCalculatedMedianIsCorrect()
        {
            List<double> testData = new List<double>()
            {
                1,2,3,4,5,6,7,8,9,10
            };

            Assert.AreEqual(testData.GetPercentile(50), 5.5);
        }
    }
}
