using GatewaysManagement.Services.Utils;
using System.Collections.Generic;

namespace GatewaysManagement.Services.Utils
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}