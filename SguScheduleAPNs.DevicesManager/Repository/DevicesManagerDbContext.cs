using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using SguScheduleAPNs.DevicesManager.Repository.Interfaces;

namespace SguScheduleAPNs.DevicesManager.Repository;

public class DevicesManagerDbContext: DbContext
{
    public DbSet<Device> Devices { get; set; }
    
    public DevicesManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}