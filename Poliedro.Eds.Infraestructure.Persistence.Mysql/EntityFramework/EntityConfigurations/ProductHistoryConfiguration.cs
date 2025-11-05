using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ProductHistory.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ProductHistoryConfiguration
{
    public ProductHistoryConfiguration(EntityTypeBuilder<ProductHistoryEntity> builder)
    {
        builder.ToTable("product_history");
        builder.HasKey(x => x.IdProductHistory);
        builder.Property(x => x.IdProductHistory).HasColumnName("id_product_history");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.OldSellPrice).HasColumnName("old_sell_price");
        builder.Property(x => x.NewSellPrice).HasColumnName("new_sell_price");
        builder.Property(x => x.OldStock).HasColumnName("old_stock");
        builder.Property(x => x.NewStock).HasColumnName("new_stock");
        builder.Property(x => x.OldPurchasePrice).HasColumnName("old_purchase_price");
        builder.Property(x => x.NewPurchasePrice).HasColumnName("new_purchase_price");
        builder.Property(x => x.Date).HasColumnName("date");
        
        // Audit fields mapping
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy");
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
    }
}
