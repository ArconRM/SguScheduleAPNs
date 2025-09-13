using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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