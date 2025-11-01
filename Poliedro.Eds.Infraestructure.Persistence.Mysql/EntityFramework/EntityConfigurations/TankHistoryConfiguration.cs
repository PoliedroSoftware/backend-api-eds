using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.TankHistory.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class TankHistoryConfiguration
{
    public TankHistoryConfiguration(EntityTypeBuilder<TankHistoryEntity> builder)
    {
        builder.ToTable("tank_history");
        builder.HasKey(x => x.IdTankHistory);
        builder.Property(x => x.IdTankHistory).HasColumnName("id_tank_history");
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.Compartment).HasColumnName("compartment");
        builder.Property(x => x.Ability).HasColumnName("ability");
        builder.Property(x => x.Stock).HasColumnName("stock");
        builder.Property(x => x.Date).HasColumnName("date");
        
        // Audit fields mapping
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy");
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
    }
}
