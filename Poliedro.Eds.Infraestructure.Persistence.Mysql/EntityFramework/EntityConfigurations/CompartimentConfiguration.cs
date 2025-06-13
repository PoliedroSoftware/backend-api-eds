using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CompartimentConfiguration
{
    public CompartimentConfiguration(EntityTypeBuilder<CompartimentEntity> builder)
    {
        builder.ToTable("compartiment");
        builder.HasKey(x => x.IdCompartment);
        builder.Property(x => x.IdCompartment).HasColumnName("id_compartiment");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.Nominal).HasColumnName("nominal");
        builder.Property(x => x.Operative).HasColumnName("operative");
        builder.Property(x => x.Stock).HasColumnName("stock");
        builder.Property(x => x.Height).HasColumnName("height");
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
    }
}