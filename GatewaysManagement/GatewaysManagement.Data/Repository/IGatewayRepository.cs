using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GatewaysManagement.Data.Repository
{
    public interface IGatewayRepository : IGenericRepository<Gateway>
    {
        bool Exists(Guid gatewayId);

        Task<Gateway> GetGatewayWithDevices(Guid gatewayId);
    }
}
