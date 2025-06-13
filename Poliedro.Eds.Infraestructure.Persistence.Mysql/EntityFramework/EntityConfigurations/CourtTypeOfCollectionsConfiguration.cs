using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    public class CourtTypeOfCollectionsConfiguration
    {
        public CourtTypeOfCollectionsConfiguration(EntityTypeBuilder<CourtTypeOfCollectionEntity> builder)
        {
            builder.ToTable("court_type_of_collection");
            builder.HasKey(x => x.IdCourtTypeOfCollection);
            builder.Property(x => x.IdCourtTypeOfCollection).HasColumnName("id_court_type_of_collection");
            builder.Property(x => x.IdCourt).HasColumnName("id_court");
            builder.Property(x => x.IdTypeOfCollection).HasColumnName("id_type_of_collection");
            builder.Property(x => x.Amount).HasColumnName("amount");
            builder.Property(x => x.Description).HasColumnName("description");

            builder.HasOne<CourtEntity>()
                .WithMany(x => x.CourtTypeOfCollections)
                .HasForeignKey(x => x.IdCourt);

            builder.HasOne(x => x.TypeOfCollection)
                      .WithMany()
                      .HasForeignKey(x => x.IdTypeOfCollection);
        }
    }
}
