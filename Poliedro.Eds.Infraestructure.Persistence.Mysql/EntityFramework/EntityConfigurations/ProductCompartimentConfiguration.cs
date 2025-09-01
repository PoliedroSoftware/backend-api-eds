using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ProductCompartimentConfiguration
{
    public ProductCompartimentConfiguration(EntityTypeBuilder<ProductCompartimentEntity> builder)
    {
        builder.ToTable("product_compartiment");
        builder.HasKey(x => x.IdProductCompartiment);
        builder.Property(x => x.IdProductCompartiment).HasColumnName("id_product_compartiment");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdCompartiment).HasColumnName("id_compartiment");
        builder.Property(x => x.Stock).HasColumnName("stock");

    }
}
