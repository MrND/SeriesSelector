using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Text;
using SeriesSelector.Frame;

namespace SeriesSelector.Data
{
    [Export(typeof(IEpisdoeService))]
    public class EpisodeService : IEpisdoeService
    {
        public IList<EpisodeType> GetSourceEpisode(string sourcePath, string fileType)
        {
            var di = Directory.GetFiles(sourcePath, "*.avi", SearchOption.AllDirectories);
            var di2 = Directory.GetFiles(sourcePath, "*.mkv", SearchOption.AllDirectories);
            var l = new ArrayList(di);
            l.AddRange(di2);

            IList<EpisodeType> episode = new List<EpisodeType>();

            foreach (string file in l)
            {
                var episodeType = new EpisodeType();
                var fileName = Path.GetFileName(file);

                if(string.IsNullOrEmpty(fileName))
                    continue;

                if(fileName.ToLower().Contains("sample"))
                    continue;

                var checker = BootStrapper.ResolveAll<IEpisodeChecker>();
                Tuple<string, string> result = null;

                foreach (var c in checker)
                {
                    result = c.CheckSeasonEpisode(fileName);
                    if (result != null)
                        break;
                }

                if (result == null)
                    continue;
                var seasonString = result.Item1;
                var episodeString = result.Item2;

                var fName = fileName.Replace(seasonString, "");
                fName = fName.Replace(episodeString, "");

                episodeType.FileName = fName;
                episodeType.Season = seasonString;
                episodeType.Episode = episodeString;
                episodeType.FullPath = file;
                episodeType.FileSize = Math.Round((((double)new FileInfo(file).Length) / 1048576), 2).ToString();

                episode.Add(episodeType);
            }
            return episode;
        }

        public Dictionary<string, string> GetMappingValues()
        {
            if (!File.Exists(Constants.MappingFilePath))
            {
                var d = new Dictionary<string, string>();
                WriteMappingValue(d);
            }

            var mappingTable = CreateMappingTable();
            mappingTable.ReadXml(Constants.MappingFilePath);
            var mappings = new Dictionary<string, string>();

            foreach (DataRow row in mappingTable.Rows)
            {
                mappings.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
            }
            return mappings;
        }

        public void WriteMappingValue(Dictionary<string, string> currentMappings)
        {
            var mappingTable = CreateMappingTable();
            foreach (var currentMapping in currentMappings)
            {
                mappingTable.Rows.Add(currentMapping.Key, currentMapping.Value);
            }

            mappingTable.WriteXml(Constants.MappingFilePath);
        }

        private DataTable CreateMappingTable()
        {
            var mappingTable = new DataTable("Mappings");
            mappingTable.Columns.Add("OldName");
            mappingTable.Columns.Add("NewName");
            return mappingTable;
        }
    }
}