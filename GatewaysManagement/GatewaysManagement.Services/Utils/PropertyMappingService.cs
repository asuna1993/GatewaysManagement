using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GatewaysManagement.Common.DTO.Response;
using GatewaysManagement.Data.Entities;
using GatewaysManagement.Services.Utils;

namespace GatewaysManagement.Services.Utils
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _gatewayPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                { "SerialNumber", new PropertyMappingValue(new List<string>() {"SerialNumber"}) },
                { "Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                { "IPAdress", new PropertyMappingValue(new List<string>() {"IPAdress"}) },
            };

        private readonly Dictionary<string, PropertyMappingValue> _devicePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"UID", new PropertyMappingValue(new List<string>() {"UID"}) },
                {"Vendor", new PropertyMappingValue(new List<string>() {"Vendor"}) },
                {"CreatedAt", new PropertyMappingValue(new List<string>() {"CreatedAt"}) },
                {"Status", new PropertyMappingValue(new List<string>() {"Status"}) },
                {"GatewayId", new PropertyMappingValue(new List<string>() {"GatewayId"}) },
                {"Gateway", new PropertyMappingValue(new List<string>() {"Gateway.Name"}) },

            };

        

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<GatewayResponse, Gateway>(_gatewayPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<DeviceResponse, Device>(_devicePropertyMapping));           
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            IEnumerable<PropertyMapping<TSource, TDestination>> matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance " + $"for <{typeof(TSource)}, {typeof(TDestination)}");
        }
    }
}