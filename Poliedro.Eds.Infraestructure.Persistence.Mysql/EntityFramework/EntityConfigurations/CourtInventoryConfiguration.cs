using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    public class CourtInventoryConfiguration
    {
        public CourtInventoryConfiguration(EntityTypeBuilder<CourtInventoryEntity> builder)
        {
            builder.ToTable("inventory");
            builder.HasKey(x => x.IdInventory);
            builder.Property(x => x.IdInventory).HasColumnName("id_inventory");
            builder.Property(x => x.Date).HasColumnName("date");
            builder.Property(x => x.ReferenceType).HasColumnName("reference_type");
            builder.Property(x => x.ReferenceId).HasColumnName("reference_id");
        }
    }
}
