using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    public class CourtDispenserConfiguration
    {
        public CourtDispenserConfiguration(EntityTypeBuilder<CourtDispenserEntity> builder)
        {
            builder.ToTable("court_dispensers");
            builder.HasKey(x => x.IdCourtDispensers);
            builder.Property(x => x.IdCourtDispensers).HasColumnName("id_court_dispensers");
            builder.Property(x => x.IdCourt).HasColumnName("id_court");
            builder.Property(x => x.AccumulatedAmount).HasColumnName("accumulated_amount");
            builder.Property(x => x.AccumulatedGallons).HasColumnName("accumulated_gallons");
            builder.Property(x => x.IdProduct).HasColumnName("id_product");
            builder.Property(x => x.IdCompartiment).HasColumnName("id_compartiment");
            builder.Property(x => x.IdHose).HasColumnName("id_hose");
            builder.HasOne<CourtEntity>()
                .WithMany(x => x.CourtDispensers)
                .HasForeignKey(x => x.IdCourt);
        }
    }
}
