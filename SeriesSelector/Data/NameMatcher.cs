using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace SeriesSelector.Data
{
    [Export(typeof(ISeriesMatcher))]
    public class NameMatcher : ISeriesMatcher
    {
        public string Match(Dictionary<string, string> mappings, string oldName)
        {
            foreach (var name in mappings.Values)
            {
                var words = name.ToLower().Split(' ');
                var wordCount = words.Length;
                var counter = words.Count(oldName.ToLower().Contains);
                if(wordCount == counter) return name;
            }
            return null;
        }
    }
}