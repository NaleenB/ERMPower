using ERMPowerDevTask.Models;
using ERMPowerDevTask.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Tests
{
    [TestClass]
    public class TestFileProcessor
    {
        [TestMethod]
        public void TestInvalidPath()
        {
            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData("fakepath");

            Assert.AreEqual(result, FileLoadResult.InvalidPath);
        }

        [TestMethod]
        public void TestNoFiles()
        {
            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData(@"C:\");

            Assert.AreEqual(result, FileLoadResult.NoFilesFound);
        }

        [TestMethod]
        public void TestSuccessfulLoad()
        {
            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData(@"C:\Users\linru\Source\Repos\ERMPower\ERMPowerDevTask\CSV");

            Assert.AreEqual(result, FileLoadResult.Success);
        }
    }
}
