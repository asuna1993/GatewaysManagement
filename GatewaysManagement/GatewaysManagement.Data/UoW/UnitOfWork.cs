using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.Repository;
using System;
using System.Threading.Tasks;

namespace GatewaysManagement.Data.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CoreDbContext _context;

        public UnitOfWork(CoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            GatewayRepository ??= new GatewayRepository(_context);
            DeviceRepository ??= new DeviceRepository(_context);
        }

        public IGatewayRepository GatewayRepository { get; set; }
        public IDeviceRepository DeviceRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}