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
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IdProductType).HasColumnName("id_product_type");
        builder.Property(x => x.PurchasePrice).HasColumnName("purchase_price");
        builder.Property(x => x.SellPrice).HasColumnName("sell_price");
        builder.Property(x => x.Stock).HasColumnName("stock");
        builder.Property(x => x.Date).HasColumnName("date");
        
        // Audit fields mapping
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy");
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
    }
}
