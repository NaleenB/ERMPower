using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Reports
{
    public class ConsoleReportWriter : IReportWriter
    {
        public void Write(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
