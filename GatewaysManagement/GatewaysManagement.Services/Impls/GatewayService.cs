using GatewaysManagement.Common.DTO.Response;
using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.UoW;
using GatewaysManagement.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewaysManagement.Services.Impls
{
    public class GatewayService : IGatewayService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPropertyMappingService _propertyMappingService;

        public GatewayService(IUnitOfWork uow, IPropertyMappingService propertyMappingService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow)); 
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public bool GatewayExists(Guid gatewayId)
        {
            return _uow.GatewayRepository.Exists(gatewayId);

        }

        public async Task<Gateway> GetGatewayAsync(Guid gatewayId)
        {
            return await _uow.GatewayRepository.FindAsync(b => b.Id == gatewayId);
        }
        
        public async Task<IEnumerable<Gateway>> GetGatewaysAsync()
        {
            return await _uow.GatewayRepository.GetAll().ToListAsync();
        }

        public async Task<PageList<Gateway>> GetGatewaysAsync(ResourceParameters resourceParameters)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            IQueryable<Gateway> gateways = _uow.GatewayRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                string searchQuery = resourceParameters.SearchQuery.Trim();
                gateways = gateways.Where(o => o.Name.Contains(searchQuery));
            }
            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                Dictionary<string, PropertyMappingValue> offerPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<GatewayResponse, Gateway>();
                gateways = gateways.ApplySort(resourceParameters.OrderBy, offerPropertyMappingDictionary);
            }
            return await PageList<Gateway>.CreateAsync(gateways, resourceParameters.PageNumber, resourceParameters.PageSize);

        }

        public async Task AddGatewayAsync(Gateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException(nameof(gateway));
            }

            gateway.Id = Guid.NewGuid();
            await _uow.GatewayRepository.AddAsync(gateway);
            await _uow.CommitAsync();
        }

        public async Task UpdateGatewayAsync(Gateway gatewayToBeUpdate)
        {
            if (gatewayToBeUpdate == null)
            {
                throw new ArgumentNullException(nameof(gatewayToBeUpdate));
            }

            _uow.GatewayRepository.Update(gatewayToBeUpdate);
            await _uow.CommitAsync();
        }
        
        public async Task DeleteGatewayAsync(Gateway gateway)
        {
            _uow.GatewayRepository.Delete(gateway);
            await _uow.CommitAsync();
        }

        public async Task<int> CountAssociatedDevices(Guid gatewayId)
        {
            var gateway = await _uow.GatewayRepository.GetGatewayWithDevices(gatewayId);

            return gateway.Devices.Count;
        }
    }
}
