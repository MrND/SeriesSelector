using System.Collections.Generic;

namespace SeriesSelector.Data
{
    public interface ISeriesMatcher
    {
        string Match(Dictionary<string,string> mappings, string oldName);
    }
}