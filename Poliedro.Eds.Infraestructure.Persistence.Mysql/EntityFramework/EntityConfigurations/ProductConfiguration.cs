using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Product.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ProductConfiguration
{
    public ProductConfiguration(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("product");
        builder.HasKey(x => x.IdProduct);
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IdProductType).HasColumnName("id_product_type");
        builder.Property(x => x.Price).HasColumnName("price");

    }
}
