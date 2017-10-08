using CsvHelper.Configuration;
using ERMPowerDevTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Helpers
{
    public sealed class CSVMapperLP : CsvClassMap<LPData>
    {
        public CSVMapperLP()
        {
            Map(m => m.DateTime).Index(3);
            Map(m => m.DataType).Index(4);
            Map(m => m.DataVal).Index(5);
        }
    }


    public sealed class CSVMapperTOU : CsvClassMap<TOUData>
    {
        public CSVMapperTOU()
        {
            Map(m => m.DateTime).Index(3);
            Map(m => m.DataType).Index(4);
            Map(m => m.Energy).Index(5);
        }
    }
}
