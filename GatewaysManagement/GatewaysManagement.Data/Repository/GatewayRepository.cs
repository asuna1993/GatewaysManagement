using GatewaysManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaysManagement.Data.Repository
{
    public class GatewayRepository: GenericRepository<Gateway>, IGatewayRepository
    {
        public GatewayRepository(CoreDbContext context)
            : base(context)
        {
        }

        public bool Exists(Guid gatewayId)
        {
            return _context.Gateways.Any(b => b.Id == gatewayId);
        }

        public async Task<Gateway> GetGatewayWithDevices(Guid gatewayId)
        {
            return await _context.Gateways.Include(o => o.Devices).SingleOrDefaultAsync(o => o.Id == gatewayId);
        }

    }
}
