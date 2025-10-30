using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class ShoppingProductViewConfiguration
{
    public ShoppingProductViewConfiguration(EntityTypeBuilder<ShoppingProductViewEntity> builder)
    {
        builder.ToTable("v_shopping_product");

        builder.HasNoKey();
        builder.Property(x => x.IdShoppingProduct).HasColumnName("id_shopping_product");
        builder.Property(x => x.IdShopping).HasColumnName("id_shopping");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Date).HasColumnName("fecha");
        builder.Property(x => x.ProductName).HasColumnName("nombre_producto");
        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.SellPrice)
            .HasColumnName("sell_price")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.PurchasePrice)
            .HasColumnName("purchase_price")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.Stock)
            .HasColumnName("stock")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.IdCompartment).HasColumnName("id_compartiment");
        builder.Property(x => x.TotalSellPrice)
            .HasColumnName("total_sell_price")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.TotalPurchasePrice)
            .HasColumnName("total_purchase_price")
            .HasColumnType("decimal(18,3)");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
    }
}
