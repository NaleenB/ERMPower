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
            FileProcessor fp = new FileProcessor("fakepath");
            var result = fp.LoadData(out List<LPFile> lpList, out List<TOUFile> touList);

            Assert.AreEqual(result, FileLoadResult.InvalidPath);
        }

        [TestMethod]
        public void TestNoFiles()
        {
            FileProcessor fp = new FileProcessor(@"C:\");
            var result = fp.LoadData(out List<LPFile> lpList, out List<TOUFile> touList);

            Assert.AreEqual(result, FileLoadResult.NoFilesFound);
        }

        [TestMethod]
        public void TestSuccessfulLoad()
        {
            FileProcessor fp = new FileProcessor(@"D:\VSProjects\repos\ERMPowerDevTask\ERMPowerDevTask\CSV");
            var result = fp.LoadData(out List<LPFile> lpList, out List<TOUFile> touList);

            Assert.AreEqual(result, FileLoadResult.Success);
        }
    }
}
