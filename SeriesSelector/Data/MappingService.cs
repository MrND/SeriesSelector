using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Windows;
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

        private DataSet _mappings;
        private DataTable _mappingTable;

        public Dictionary<string, string> GetMappingValues()
        {
            _mappings.Tables.Add(_mappingTable);
            _mappings.ReadXml(Settings.Default.MappingPath);
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

            _mappings.Tables.Add(_mappingTable);
            _mappings.WriteXml(Settings.Default.MappingPath);
        }
    }
}