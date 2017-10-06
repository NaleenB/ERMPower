using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Models
{
    public class LPFile
    {
        public string FileName { get; set; }

        public List<LPData> LPDataList { get; set; }
    }
}
