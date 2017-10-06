using CsvHelper;
using CsvHelper.Configuration;
using ERMPowerDevTask.Helpers;
using ERMPowerDevTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMPowerDevTask.Processors
{
    public class FileProcessor
    {
        private readonly string _filesPath;

        public FileProcessor(string filesPath)
        {
            _filesPath = filesPath;
        }


        public FileLoadResult LoadData(out List<LPFile> lpFiles, out List<TOUFile> touFiles)
        {
            lpFiles = new List<LPFile>();
            touFiles = new List<TOUFile>();

            string[] filesInFolder;
            try
            {
                filesInFolder = Directory.GetFiles(_filesPath, "*.csv", SearchOption.TopDirectoryOnly);
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
                        LPDataList = csv.GetRecords<LPData>().ToList()
                    });
                }
                else if (fi.Name.Substring(0, 2).ToUpper() == "TO")
                {
                    touFiles.Add(new TOUFile
                    {
                        FileName = fi.Name,
                        TOUDataList = csv.GetRecords<TOUData>().ToList()
                    });
                }

                tr.Close();
            }

            return FileLoadResult.Success;
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
