using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CourtDispensersInventoryConfiguration
{
    public CourtDispensersInventoryConfiguration(EntityTypeBuilder<CourtDispensersInventoryEntity> builder)
    {
        builder.ToTable("court_dispensers_inventory");
        builder.HasKey(x => x.IdCourtDispensersInventory);
        builder.Property(x => x.IdCourtDispensersInventory).HasColumnName("id_court_dispensers_inventorycol");
        builder.Property(x => x.IdCourtdDispensers).HasColumnName("id_court_dispensers");
        builder.Property(x => x.IdInventory).HasColumnName("id_inventory");
    }
}
