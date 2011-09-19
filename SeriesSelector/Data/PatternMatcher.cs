using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SeriesSelector.Data
{
    [Export(typeof(ISeriesMatcher))]
    public class PatternMatcher : ISeriesMatcher
    {
        public string Match(Dictionary<string,string> mappings, string oldName)
        {
            string result;
            mappings.TryGetValue(oldName, out result);
            return result;
        }
    }
}