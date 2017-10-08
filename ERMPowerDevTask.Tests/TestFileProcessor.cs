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
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData(rootPath);

            Assert.AreEqual(result, FileLoadResult.Success);
        }

        [TestMethod]
        public void TestRequiredDataIsLoadedFromSampleFile()
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData(rootPath);

            Assert.AreEqual(result, FileLoadResult.Success);

            Assert.AreEqual(fp.lpFiles[0].LPDataList.Count, 384);
        }

        [TestMethod]
        public void TestRightDataIsFilteredFromSampleFile()
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            MedianProcessor fp = new MedianProcessor();
            var result = fp.LoadFileData(rootPath);

            Assert.AreEqual(result, FileLoadResult.Success);

            fp.ProcessData();

            Assert.AreEqual(fp.lpFiles[0].LPDataList.Count, 96);
        }
    }
}
