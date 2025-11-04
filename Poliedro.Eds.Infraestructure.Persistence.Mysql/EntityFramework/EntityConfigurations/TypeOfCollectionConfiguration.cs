using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class TypeOfCollectionConfiguration
{
    public TypeOfCollectionConfiguration(EntityTypeBuilder<TypeOfCollectionEntity> builder)
    {
        // map to new table name
        builder.ToTable("method_payment");
        builder.HasKey(x => x.IdTypeOfCollection);
        builder.Property(x => x.IdTypeOfCollection).HasColumnName("id_type_of_collection");
        builder.Property(x => x.Description).HasColumnName("description");

        // map id_eds
        builder.Property(x => x.IdEds).HasColumnName("id_eds").IsRequired(false);
        builder.HasIndex(x => x.IdEds).HasDatabaseName("idx_method_payment_id_eds");
        builder.HasOne<EdsEntity>()
               .WithMany()
               .HasForeignKey(x => x.IdEds)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
