using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ProductTypeConfiguration
{
    public ProductTypeConfiguration(EntityTypeBuilder<ProductTypeEntity> builder)
    {
        builder.ToTable("product_type");
        builder.HasKey(x => x.IdProductType);
        builder.Property(x => x.IdProductType).HasColumnName("id_product_type");
        builder.Property(x => x.Description).HasColumnName("description");

    }
}
