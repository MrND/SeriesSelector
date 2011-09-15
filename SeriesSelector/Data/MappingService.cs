using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Windows;
using SeriesSelector.Frame;
using SeriesSelector.Properties;

namespace SeriesSelector.Data
{
    [Export(typeof(IMappingService))]
    public class MappingService : IMappingService
    {

        
        public MappingService()
        {
            _mappingTable.Columns.Add("OldName");
            _mappingTable.Columns.Add("NewName");
        }
        private DataTable _mappingTable;

        public Dictionary<string, string> GetMappingValues()
        {
            if (!File.Exists(Constants.MappingFilePath))
            {
                var d = new Dictionary<string, string>();
                WriteMappingValue(d);
            }
            
            _mappingTable.ReadXml(Constants.MappingFilePath);
            Dictionary<string, string> mappings = null;

            foreach (DataRow row in _mappingTable.Rows)
            {
                mappings.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
            }
            return mappings;
        }

        public void WriteMappingValue(Dictionary<string, string> currentMappings)
        {
            foreach (var currentMapping in currentMappings)
            {
                _mappingTable.Rows.Add(currentMapping.Key, currentMapping.Value);
            }

            _mappingTable.WriteXml(Constants.MappingFilePath);
        }
    }
}