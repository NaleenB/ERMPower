using CsvHelper;
using ERMPowerDevTask.Helpers;
using ERMPowerDevTask.Models;
using ERMPowerDevTask.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        /// <summary>
        /// Print additional console logs
        /// </summary>
        private bool enableAdditionalConsoleLogs = true;

        /// <summary>
        /// Interested data type names that are considered for processing
        /// If different files have different data type names, they are all added here
        /// Not case sensitive
        /// </summary>
        private readonly string[] interestedDataTypeIdentifiers = new string[]
        {
            "Export Wh Total",
            "Export KWh Total",
            "Export MWh Total",
            "TOU CH1 Export Wh Total"
        };

        #region Interface Implementation

        /// <summary>
        /// Read all CSV in the path provided
        /// If they conform to the prefix "LP" or "TO" load data in to data model
        /// Return appropriate success / fail result
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
                TextReader tr;

                try
                {
                    tr = File.OpenText(file);
                }
                catch (IOException)
                {
                    return FileLoadResult.FileUsedByAnotherProgram;
                }
                catch (Exception)
                {
                    throw;
                }

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

        /// <summary>
        /// Logic for processing data prior to generating reports
        /// </summary>
        public void ProcessData()
        {
            // From all the records in the files, required data only is filtered
            FilterData();

            // Median of data values is calculated for each file
            CalculateMedian();
        }

        public void PrintReports(IReportWriter reportSource)
        {
            LogToConsole("");
            LogToConsole("---Printing Report (abnormal values)---");

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

        #endregion

        #region Private Methods

        /// <summary>
        /// From all the records in the files, required data only is filtered
        /// </summary>
        private void FilterData()
        {
            foreach (var lpFile in lpFiles)
            {
                int before = lpFile.LPDataList.Count;
                lpFile.LPDataList = lpFile.LPDataList.Where(x => interestedDataTypeIdentifiers.Contains(x.DataType, StringComparer.OrdinalIgnoreCase)).ToList();
                int after = lpFile.LPDataList.Count;

                LogToConsole(string.Format("In {0} {1} out of {2} records filtered", lpFile.FileName, after, before));
            }

            foreach (var touFile in touFiles)
            {
                int before = touFile.TOUDataList.Count;
                touFile.TOUDataList = touFile.TOUDataList.Where(x => interestedDataTypeIdentifiers.Contains(x.DataType, StringComparer.OrdinalIgnoreCase)).ToList();
                int after = touFile.TOUDataList.Count;

                LogToConsole(string.Format("In {0} {1} out of {2} records filtered", touFile.FileName, after, before));
            }
        }

        /// <summary>
        /// Median of data values is calculated for each file
        /// </summary>
        private void CalculateMedian()
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

        private void LogToConsole(string msg)
        {
            if (enableAdditionalConsoleLogs)
            {
                Console.WriteLine(msg);
            }
        }

        #endregion

    }
}
