using System;
using System.ComponentModel.Composition;

namespace SeriesSelector.Data
{
    [Export(typeof(IEpisodeChecker))]
    public class SimpleEpisodeChecker : IEpisodeChecker
    {
        public Tuple<string, string> CheckSeasonEpisode(string fileName)
        {
            string seasonString = null;
            string episodeString = null;
            string[] array = { fileName };
            for (int i = 0; i < array.Length; i++)
            {
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

            if (!string.IsNullOrEmpty(seasonString) && !string.IsNullOrEmpty(episodeString))
                return new Tuple<string, string>(seasonString, episodeString);
            return null;
        }
    }
}