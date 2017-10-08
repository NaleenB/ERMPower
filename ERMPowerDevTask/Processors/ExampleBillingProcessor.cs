using ERMPowerDevTask.Models;
using ERMPowerDevTask.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Processors
{
    public class ExampleBillingProcessor : IFileProcessor
    {
        public FileLoadResult LoadFileData(string path)
        {
            // Logic to load billing files goes here
            throw new NotImplementedException();
        }

        public void ProcessData()
        {
            // Logic to process loaded data goes here
            throw new NotImplementedException();
        }

        public void PrintReports(IReportWriter reportSource)
        {
            // Logic to generate and print billing reports goes here
            throw new NotImplementedException();
        }

        
    }
}
