using GatewaysManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Device> GetDeviceWithGateway(Guid deviceId)
        {
            return await _context.Devices.Include(o => o.Gateway).SingleOrDefaultAsync(o => o.Id == deviceId);
        }
    }
}
