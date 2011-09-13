using System.Collections.Generic;

namespace SeriesSelector.Data
{
    public interface IEpisdoeService
    {
        IList<EpisodeType> GetSourceEpisode(string sourcePath, string fileType);
        Dictionary<string, string> GetMappingValues();
        void WriteMappingValue(Dictionary<string, string> currentMappings);
    }
}