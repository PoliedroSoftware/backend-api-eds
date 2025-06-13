using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class TypeOfCollectionConfiguration
{
    public TypeOfCollectionConfiguration(EntityTypeBuilder<TypeOfCollectionEntity> builder)
    {
        builder.ToTable("type_of_collection");
        builder.HasKey(x => x.IdTypeOfCollection);
        builder.Property(x => x.IdTypeOfCollection).HasColumnName("id_type_of_collection");
        builder.Property(x => x.Description).HasColumnName("description");

    }
}
