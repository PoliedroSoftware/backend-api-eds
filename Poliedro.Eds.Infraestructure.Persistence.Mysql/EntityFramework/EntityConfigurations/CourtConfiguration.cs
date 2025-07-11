using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Court.Entities;


namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CourtConfiguration
{
    public CourtConfiguration(EntityTypeBuilder<CourtEntity> builder)
    {
        builder.ToTable("court");
        builder.HasKey(x => x.IdCourt);
        builder.Property(x => x.IdCourt).HasColumnName("id_court");
        builder.Property(x => x.IdIslander).HasColumnName("id_islander");
        builder.Property(x => x.DateStarttime).HasColumnName("date_starttime");
        builder.Property(x => x.Starttime).HasColumnName("starttime");
        builder.Property(x => x.DateEndtime).HasColumnName("date_endtime");
        builder.Property(x => x.Endtime).HasColumnName("endtime");
        builder.Property(x => x.Consecutive).HasColumnName("consecutive");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.Descripcion).HasColumnName("descripcion");
        builder.Property(x => x.Distintic).HasColumnName("distintic");

        builder.HasMany(x => x.CourtDispensers)
       .WithOne()
       .HasForeignKey(x => x.IdCourt);

        builder.HasMany(x => x.CourtDocuments)
               .WithOne()
               .HasForeignKey(x => x.IdCourt);

        builder.HasMany(x => x.CourtExpenditures)
               .WithOne()
               .HasForeignKey(x => x.IdCourt);

        builder.HasMany(x => x.CourtTypeOfCollections)
               .WithOne()
               .HasForeignKey(x => x.IdCourt);
    }
}
