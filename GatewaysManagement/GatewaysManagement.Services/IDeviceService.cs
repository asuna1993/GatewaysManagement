using GatewaysManagement.Data.Entities;
using GatewaysManagement.Services.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GatewaysManagement.Services
{
    public interface IDeviceService
    {
        bool DeviceExists(Guid deviceId);
        Task<Device> GetDeviceAsync(Guid deviceId);
        Task<IEnumerable<Device>> GetDevicesAsync(Guid gatewayId);
        Task<PageList<Device>> GetDevicesAsync(Guid gatewayId, ResourceParameters parameters);
        Task AddDeviceAsync(Device device);
        Task UpdateDeviceAsync(Device deviceToBeUpdate);
        Task DeleteDeviceAsync(Device device);
        Task<bool> ValidUID(int UID);
        Task<bool> ValidUID(Guid deviceId, int UID);

    }
}
