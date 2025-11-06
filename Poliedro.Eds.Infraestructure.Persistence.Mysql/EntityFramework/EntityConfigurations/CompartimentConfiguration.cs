using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CompartimentConfiguration
{
    public CompartimentConfiguration(EntityTypeBuilder<CompartimentEntity> builder)
    {
        builder.ToTable("compartiment");
        builder.HasKey(x => x.IdCompartiment);
        builder.Property(x => x.IdCompartiment).HasColumnName("id_compartiment");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.Nominal)
            .HasColumnName("nominal")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.Operative)
            .HasColumnName("operative")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.Height)
            .HasColumnName("height")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
