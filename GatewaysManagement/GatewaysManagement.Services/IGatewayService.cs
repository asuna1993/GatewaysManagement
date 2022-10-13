using GatewaysManagement.Data.Entities;
using GatewaysManagement.Services.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GatewaysManagement.Services
{
    public interface IGatewayService
    {
        bool GatewayExists(Guid gatewayId);
        Task<Gateway> GetGatewayAsync(Guid gatewayId);
        Task<IEnumerable<Gateway>> GetGatewaysAsync();
        Task<PageList<Gateway>> GetGatewaysAsync(ResourceParameters resourceParameters);
        Task AddGatewayAsync(Gateway gateway);
        Task UpdateGatewayAsync(Gateway gatewayToBeUpdate);
        Task DeleteGatewayAsync(Gateway gateway);
        Task<int> CountAssociatedDevices(Guid gatewayId);

    }
}
