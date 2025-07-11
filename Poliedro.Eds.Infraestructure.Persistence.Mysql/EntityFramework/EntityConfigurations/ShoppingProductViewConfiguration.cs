using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ShoppingProductView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ShoppingProductViewConfiguration
{
    public ShoppingProductViewConfiguration(EntityTypeBuilder<ShoppingProductViewEntity> builder)
    {
        builder.ToTable("v_shopping_product");

        builder.HasNoKey();
        builder.Property(x => x.IdShoppingProduct).HasColumnName("id_shopping_product");
        builder.Property(x => x.IdShopping).HasColumnName("id_shopping");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.ProductName).HasColumnName("nombre_producto");
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        builder.Property(x => x.Price).HasColumnName("price");
        builder.Property(x => x.TotalPrice).HasColumnName("total_precio");
        builder.Property(x => x.IdCompartment).HasColumnName("id_compartiment");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");

    }
}