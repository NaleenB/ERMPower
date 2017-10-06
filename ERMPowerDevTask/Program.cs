using ERMPowerDevTask.Processors;
using ERMPowerDevTask.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask
{
    class Program
    {
        static void Main(string[] args)
        {
            // Required report writer is intialized to be dependency injected
            IReportWriter reportWriter = new ConsoleReportWriter();

            // CSV file path is retrieved through configuration
            string csvPath = ConfigurationManager.AppSettings["CSVLocation"];

            // Required file processor is initialized
            IFileProcessor fileProcessor = new MedianProcessor();
                        
            // load data in to class objects
            fileProcessor.LoadFileData(csvPath);

            // process data in the class objects
            fileProcessor.ProcessData();

            // print reports
            fileProcessor.PrintReports(reportWriter);

            Console.ReadLine();
        }
    }
}
