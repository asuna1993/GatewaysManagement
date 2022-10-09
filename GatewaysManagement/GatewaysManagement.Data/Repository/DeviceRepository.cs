using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GatewaysManagement.Data.Repository
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(CoreDbContext context)
            : base(context)
        {
        }

        public bool Exists(Guid deviceId)
        {
            return _context.Devices.Any(b => b.Id == deviceId);
        }
    }
}
