using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class TypeOfCollectionViewConfiguration
{
    public TypeOfCollectionViewConfiguration(EntityTypeBuilder<TypeOfCollectionViewEntity> builder)
    {
        builder.ToTable("v_type_of_collection");
        builder.HasKey(x => x.IdTypeOfCollection);
        builder.Property(x => x.IdTypeOfCollection).HasColumnName("id_type_of_collection");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.Date).HasColumnName("date");

    }
}