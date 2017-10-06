using CsvHelper;
using CsvHelper.Configuration;
using ERMPowerDevTask.Helpers;
using ERMPowerDevTask.Models;
using ERMPowerDevTask.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Processors
{
    public class MedianProcessor : IFileProcessor
    {
        /// <summary>
        /// Represents list of LP files and their data
        /// </summary>
        public List<LPFile> lpFiles;

        /// <summary>
        /// Represents list of TOU files and their data
        /// </summary>
        public List<TOUFile> touFiles;

        public FileLoadResult LoadFileData(string path)
        {
            lpFiles = new List<LPFile>();
            touFiles = new List<TOUFile>();

            string[] filesInFolder;
            try
            {
                filesInFolder = Directory.GetFiles(path, "*.csv", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                return FileLoadResult.InvalidPath;
            }
            catch (Exception)
            {
                throw;
            }

            if (filesInFolder.Length == 0)
            {
                return FileLoadResult.NoFilesFound;
            }

            foreach (var file in filesInFolder)
            {
                FileInfo fi = new FileInfo(file);
                TextReader tr = File.OpenText(file);

                var csv = new CsvReader(tr);
                csv.Configuration.RegisterClassMap<CSVMapperLP>();
                csv.Configuration.RegisterClassMap<CSVMapperTOU>();

                if (fi.Name.Substring(0, 2).ToUpper() == "LP")
                {
                    lpFiles.Add(new LPFile
                    {
                        FileName = fi.Name,
                        LPDataList = csv.GetRecords<LPData>().OrderBy(x => x.DataVal).ToList()
                    });
                }
                else if (fi.Name.Substring(0, 2).ToUpper() == "TO")
                {
                    touFiles.Add(new TOUFile
                    {
                        FileName = fi.Name,
                        TOUDataList = csv.GetRecords<TOUData>().OrderBy(x => x.Energy).ToList()
                    });
                }

                tr.Close();
            }

            return FileLoadResult.Success;
        }

        public void ProcessData()
        {
            foreach (var lpFile in lpFiles)
            {
                lpFile.Median = lpFile.LPDataList.Select(x => x.DataVal).GetPercentile(50);
            }

            foreach (var touFile in touFiles)
            {
                touFile.Median = touFile.TOUDataList.Select(x => x.Energy).GetPercentile(50);
            }
        }

        public void PrintReports(IReportWriter reportSource)
        {
            foreach (var lpFile in lpFiles)
            {
                double lowerBoundary = lpFile.Median * .8;
                double upperBoundary = lpFile.Median * 1.2;

                int count = lpFile.LPDataList.Where(x => x.DataVal < lowerBoundary || x.DataVal > upperBoundary).Count();
                foreach (var lpData in lpFile.LPDataList)
                {
                    if (lpData.DataVal < lowerBoundary || lpData.DataVal > upperBoundary)
                    {
                        reportSource.Write(string.Format("{0} {1} {2} {3}", lpFile.FileName, lpData.DateTime, lpData.DataVal, lpFile.Median));
                    }
                }
            }

            foreach (var touFile in touFiles)
            {
                double lowerBoundary = touFile.Median * .8;
                double upperBoundary = touFile.Median * 1.2;

                int count = touFile.TOUDataList.Where(x => x.Energy < lowerBoundary || x.Energy > upperBoundary).Count();
                foreach (var touData in touFile.TOUDataList)
                {
                    if (touData.Energy < lowerBoundary || touData.Energy > upperBoundary)
                    {
                        reportSource.Write(string.Format("{0} {1} {2} {3}", touFile.FileName, touData.DateTime, touData.Energy, touFile.Median));
                    }
                }
            }
        }


    }

    public enum FileLoadResult
    {
        NoFilesFound,
        CorruptedData,
        InvalidPath,
        Success
    }
}
