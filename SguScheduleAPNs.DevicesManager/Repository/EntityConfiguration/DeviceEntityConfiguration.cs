using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SguScheduleAPNs.DevicesManager.Entities;

namespace SguScheduleAPNs.DevicesManager.Repository.EntityConfiguration;

public class DeviceEntityConfiguration: IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(lp => lp.Uuid);
        
        builder.Property(lp => lp.Uuid)
            .ValueGeneratedOnAdd();
    }
}