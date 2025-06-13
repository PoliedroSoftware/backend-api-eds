using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CompartimentCapacityConfiguration
{
    public CompartimentCapacityConfiguration(EntityTypeBuilder<CompartimentCapacityEntity> builder)
    {
        builder.ToTable("compartiment_capacity");
        builder.HasKey(x => x.IdCompartimentCapacity);
        builder.Property(x => x.IdCompartimentCapacity).HasColumnName("id_compartiment_capacity");
        builder.Property(x => x.IdCompartiment).HasColumnName("id_compartiment");
        builder.Property(x => x.IdCapacity).HasColumnName("id_capacity");
        builder.Property(x => x.Default).HasColumnName("default");

    }
}
