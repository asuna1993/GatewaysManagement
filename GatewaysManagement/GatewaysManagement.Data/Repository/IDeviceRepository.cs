using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Data.Repository
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        bool Exists(Guid deviceId);
    }
}
