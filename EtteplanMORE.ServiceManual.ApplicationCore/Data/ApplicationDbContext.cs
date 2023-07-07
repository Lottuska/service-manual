using Microsoft.EntityFrameworkCore;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FactoryDevice> FactoryDevices { get; set; }
        public DbSet<MaintenanceTask> MaintenanceTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceTask>()
                .HasOne(mt => mt.FactoryDevice)
                .WithMany(fd => fd.MaintenanceTasks)
                .HasForeignKey(mt => mt.DeviceId);
        }
    }
}