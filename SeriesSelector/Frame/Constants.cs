using System;
using System.IO;
using System.Reflection;

namespace SeriesSelector.Frame
{
    public static class Constants
    {
        public static string MappingFilePath = new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "Mappings.xml")).LocalPath;
    }
}