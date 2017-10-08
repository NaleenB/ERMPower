using ERMPowerDevTask.Models;
using ERMPowerDevTask.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Processors
{
    interface IFileProcessor
    {
        FileLoadResult LoadFileData(string path);

        void ProcessData();

        void PrintReports(IReportWriter reportSource);
    }
}
