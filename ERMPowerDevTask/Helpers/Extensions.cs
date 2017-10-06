using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Median method copied from
        /// https://stackoverflow.com/a/5275324
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double GetPercentile(this IEnumerable<double> data, double excelPercentile)
        {
            double[] sequence = data.ToArray();
            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * excelPercentile / 100 + 1;
            // Another method: double n = (N + 1) * excelPercentile;
            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }
    }
}
