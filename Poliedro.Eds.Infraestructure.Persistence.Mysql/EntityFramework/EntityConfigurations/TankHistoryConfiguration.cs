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
        builder.Property(x => x.OldAbility).HasColumnName("old_ability");
        builder.Property(x => x.NewAbility).HasColumnName("new_ability");
        
        // Audit fields mapping
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy");
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
    }
}
