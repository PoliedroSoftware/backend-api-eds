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
        
        // Configure nullable properties
        builder.Property(x => x.PurchasePrice)
            .HasColumnName("purchase_price")
            .IsRequired(false);
            
        builder.Property(x => x.SellPrice)
            .HasColumnName("sell_price")
            .IsRequired(false);
            
        builder.Property(x => x.Stock)
            .HasColumnName("stock")
            .IsRequired(false);
            
        builder.Property(x => x.Date).HasColumnName("date");

        // Configure relationship with ProductType
        builder.HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.IdProductType)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
