using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Text;
using SeriesSelector.Properties;

namespace SeriesSelector.Data
{
    [Export(typeof(IEpisdoeService))]
    public class EpisodeService : IEpisdoeService
    {
        public IList<EpisodeType> GetSourceEpisode(string sourcePath, string fileType)
        {
            var di = Directory.GetFiles(sourcePath, fileType, SearchOption.AllDirectories);
            
            string[] fileArray;

            char[] delimiter = "\\".ToCharArray();

            var sb = new StringBuilder();
            
            IList<EpisodeType> episode = new List<EpisodeType>();

            foreach (var file in di)
            {
                var episodeType = new EpisodeType();
                string seasonString = null;
                string episodeString = null;
                fileArray = file.Split(delimiter);
                string fileName = fileArray.GetValue(fileArray.Length-1).ToString();

                string[] array = {fileName};
                for (int i = 0; i < array.Length; i++)
                {
                    sb.AppendLine();
                    char[] chars = array[i].ToCharArray();
                    for (int j = 0; j < chars.Length; j++)
                    {
                        if (chars[j] == 's' || chars[j] == 'S')
                        {
                            int number = 0;
                            if (int.TryParse(chars[j + 1] + "" + chars[j + 2], out number))
                            {
                                string str = chars[j] + "" + chars[j + 1] + "" + chars[j + 2];
                                seasonString = str;
                            }
                        }
                    }
                }

                for (int i = 0; i < array.Length; i++)
                {
                    sb.AppendLine();
                    char[] chars = array[i].ToCharArray();
                    for (int j = 0; j < chars.Length; j++)
                    {
                        if (chars[j] == 'e' || chars[j] == 'E')
                        {
                            int number = 0;
                            if (int.TryParse(chars[j + 1] + "" + chars[j + 2], out number))
                            {
                                string str = chars[j] + "" + chars[j + 1] + "" + chars[j + 2];
                                episodeString = str;
                            }
                        }
                    }
                }
                if (seasonString != null && episodeString!= null)
                {
                    string fName;
                    int found1 = fileName.IndexOf(seasonString);
                    fName = fileName.Remove(found1, 3);

                    int found2 = fName.IndexOf(episodeString);
                    fName = fName.Remove(found2, 3);

                    int found3 = fName.IndexOf("sample");
                    if (found3 <= 0)
                    {
                        episodeType.FileName = fName;
                        episodeType.Season = seasonString;
                        episodeType.Episode = episodeString;
                        episodeType.FullPath = file;

                        episode.Add(episodeType);
                    }
                }
            }
            return episode;
        }

        public Dictionary<string, string> GetMappingValues()
        {
            var mappingsDs = new DataSet();
            var mappingTable = new DataTable();
            mappingTable.Columns.Add("OldName");
            mappingTable.Columns.Add("NewName");
            mappingsDs.Tables.Add(mappingTable);
            mappingsDs.ReadXml(Settings.Default.MappingPath);
            var mappings = new Dictionary<string, string>();

            foreach (DataRow row in mappingTable.Rows)
            {
                mappings.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
            }
            return mappings;
        }

        public void WriteMappingValue(Dictionary<string, string> currentMappings)
        {
            var mappings = new DataSet();
            var mappingTable = new DataTable();
            mappingTable.Columns.Add("OldName");
            mappingTable.Columns.Add("NewName");
            foreach (var currentMapping in currentMappings)
            {
                mappingTable.Rows.Add(currentMapping.Key, currentMapping.Value);
            }

            mappings.Tables.Add(mappingTable);
            mappings.WriteXml(Settings.Default.MappingPath);
        }
    }
}