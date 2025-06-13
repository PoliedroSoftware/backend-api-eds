using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CapacityConfiguration
{
    public CapacityConfiguration(EntityTypeBuilder<CapacityEntity> builder)
    {
        builder.ToTable("capacity");
        builder.HasKey(x => x.IdCapacity);
        builder.Property(x => x.IdCapacity).HasColumnName("id_capacity");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Height).HasColumnName("Height");
        builder.Property(x => x.Gallon).HasColumnName("Gallon");
        builder.Property(x => x.Liters).HasColumnName("Liters");
    }
}
