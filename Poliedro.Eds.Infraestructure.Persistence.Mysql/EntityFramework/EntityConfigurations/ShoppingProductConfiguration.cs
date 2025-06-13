using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ShoppingProductConfiguration
{
    public ShoppingProductConfiguration(EntityTypeBuilder<ShoppingProductEntity> builder)
    {
        builder.ToTable("shopping_product");
        builder.HasKey(x => x.IdShoppingProduct);
        builder.Property(x => x.IdShoppingProduct).HasColumnName("id_shopping_product");
        builder.Property(x => x.IdShopping).HasColumnName("id_shopping");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        builder.Property(x => x.Price).HasColumnName("price");
        builder.Property(x => x.IdCompartment).HasColumnName("id_compartiment");
    }
}