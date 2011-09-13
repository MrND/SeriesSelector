using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml;

namespace Data
{
    [Export(typeof(IMappingService))]
    public class MappingService : IMappingService
    {
        public IList<MappingValue> GetMappingValues()
        {
            IList<MappingValue> values = null;
            XmlReader reader = XmlReader.Create(@"E:\My Files\Dokumente\SeriesSelectorResources\Mappings.xml");
            var value = new MappingValue();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "OldName")
                    value.OldName = reader.ReadElementString();
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "NewName")
                {
                    value.NewName = reader.ReadElementString();
                    values.Add(value);
                }
            }

            return values;
        }
    }
}