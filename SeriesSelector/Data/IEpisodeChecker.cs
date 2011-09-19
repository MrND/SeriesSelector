using System;

namespace SeriesSelector.Data
{
    public interface IEpisodeChecker
    {
        Tuple<string, string> CheckSeasonEpisode(string fileName);
    }
}