using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
