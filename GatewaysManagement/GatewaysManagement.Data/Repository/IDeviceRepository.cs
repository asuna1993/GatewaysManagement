using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GatewaysManagement.Data.Repository
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        bool Exists(Guid deviceId);
        Task<Device> GetDeviceWithGateway(Guid deviceId);
    }
}
