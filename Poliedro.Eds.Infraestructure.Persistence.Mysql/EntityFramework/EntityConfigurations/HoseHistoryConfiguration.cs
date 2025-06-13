using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class HoseHistoryConfiguration
{
    public HoseHistoryConfiguration(EntityTypeBuilder<HoseHistoryEntity> builder)
    {
        builder.ToTable("hose_history");
        builder.HasKey(x => x.IdHoseHistory);
        builder.Property(x => x.IdHoseHistory).HasColumnName("id_hose_hose_history");
        builder.Property(x => x.IdHose).HasColumnName("id_hose");
        builder.Property(x => x.AccumulatedAmount).HasColumnName("accumulated_amount");
        builder.Property(x => x.AccumulatedGallons).HasColumnName("accumulatd_gallons");
        builder.Property(x => x.IdDispensers).HasColumnName("id_dispensers");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
