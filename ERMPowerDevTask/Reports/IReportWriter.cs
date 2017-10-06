using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Reports
{
    public interface IReportWriter
    {
        void Write(string msg);
    }
}
