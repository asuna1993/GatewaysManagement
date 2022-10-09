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
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPropertyMappingService _propertyMappingService;

        public DeviceService(IUnitOfWork uow, IPropertyMappingService propertyMappingService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public bool DeviceExists(Guid gatewayId)
        {
            return _uow.DeviceRepository.Exists(gatewayId);

        }
        public async Task<Device> GetDeviceAsync(Guid deviceId)
        {
            return await _uow.DeviceRepository.FindAsync(b => b.Id == deviceId);
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync(Guid gatewayId)
        {
            return await _uow.DeviceRepository.FindAllAsync(b => b.GatewayId == gatewayId);
        }

        public async Task<PageList<Device>> GetDevicesAsync(Guid gatewayId, ResourceParameters resourceParameters)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            IQueryable<Device> devices = _uow.DeviceRepository.FindBy(b => b.GatewayId == gatewayId);

            if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                string searchQuery = resourceParameters.SearchQuery.Trim();
                devices = devices.Where(o => o.Vendor.Contains(searchQuery));
            }
            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                Dictionary<string, PropertyMappingValue> offerPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<DeviceResponse, Device>();
                devices = devices.ApplySort(resourceParameters.OrderBy, offerPropertyMappingDictionary);
            }
            return await PageList<Device>.CreateAsync(devices, resourceParameters.PageNumber, resourceParameters.PageSize);

        }

        public async Task AddDeviceAsync(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            device.Id = Guid.NewGuid();
            device.CreatedAt = DateTime.UtcNow;
            await _uow.DeviceRepository.AddAsync(device);
            await _uow.CommitAsync();
        }

        public async Task UpdateDeviceAsync(Guid deviceId, Device deviceToBeUpdate)
        {
            if (deviceToBeUpdate == null)
            {
                throw new ArgumentNullException(nameof(deviceToBeUpdate));
            }
            await _uow.DeviceRepository.UpdateAsync(deviceToBeUpdate, deviceId);
            await _uow.CommitAsync();
        }

        public async Task DeleteDeviceAsync(Device device)
        {
            _uow.DeviceRepository.Delete(device);
            await _uow.CommitAsync();
        }
    }
}
