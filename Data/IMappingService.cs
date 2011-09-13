using System.Collections.Generic;

namespace Data
{
    public interface IMappingService
    {
        IList<MappingValue> GetMappingValues();
    }
}