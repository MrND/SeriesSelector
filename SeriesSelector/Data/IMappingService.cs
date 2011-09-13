using System.Collections.Generic;

namespace SeriesSelector.Data
{
    public interface IMappingService
    {
        Dictionary<string, string> GetMappingValues();
        void WriteMappingValue(Dictionary<string, string> currentMappings);
    }
}