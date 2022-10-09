using GatewaysManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GatewaysManagement.Data
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Device> Devices { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Gateway>()
                   .HasIndex(d => d.SerialNumber)
                   .IsUnique();

            builder.Entity<Device>()
                   .HasIndex(d => d.UID)
                   .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}