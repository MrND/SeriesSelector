using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace SeriesSelector.Data
{
    [Export(typeof(IEpisodeChecker))]
    public class ThreeDigitsEpisodeChecker : IEpisodeChecker
    {
        public Tuple<string, string> CheckSeasonEpisode(string fileName)
        {
            var result = Regex.Match(fileName, @"\d\d\d");
            return result.ToString() == "720"
                       ? null
                       : new Tuple<string, string>(string.Format("S0{0}", result.ToString().Substring(0, 1)),
                                                   string.Format("E{0}", result.ToString().Substring(1, 2)));
        }
    }
}