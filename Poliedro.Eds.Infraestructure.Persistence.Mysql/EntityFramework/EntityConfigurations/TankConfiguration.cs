using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class TankConfiguration
{
    public TankConfiguration(EntityTypeBuilder<TankEntity> builder)
    {
        builder.ToTable("tank");
        builder.HasKey(x => x.IdTank);
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.Ability).HasColumnName("ability");
        builder.Property(x => x.Compartment).HasColumnName("compartment");
        builder.Property(x => x.Stock).HasColumnName("stock");
    }
}
