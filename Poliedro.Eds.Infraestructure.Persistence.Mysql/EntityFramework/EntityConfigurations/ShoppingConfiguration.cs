using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ShoppingConfiguration
{
    public ShoppingConfiguration(EntityTypeBuilder<ShoppingEntity> builder)
    {
        builder.ToTable("shopping");
        builder.HasKey(x => x.IdShopping);
        builder.Property(x => x.IdShopping).HasColumnName("id_shopping");
        builder.Property(x => x.Invoice).HasColumnName("invoice");
        builder.Property(x => x.Date).HasColumnName("date");
        builder.Property(x => x.IdProvider).HasColumnName("id_provider");
        builder.Property(x => x.IdCategory).HasColumnName("id_category");
        builder.Property(x => x.Amount).HasColumnName("amount");

        // map id_eds
        builder.Property(x => x.IdEds).HasColumnName("id_eds").IsRequired(false);
        builder.HasIndex(x => x.IdEds).HasDatabaseName("idx_shopping_id_eds");
        builder.HasOne<EdsEntity>()
               .WithMany()
               .HasForeignKey(x => x.IdEds)
               .OnDelete(DeleteBehavior.Restrict);

        // Configure Provider relationship
        builder.HasOne(x => x.Provider)
               .WithMany()
               .HasForeignKey(x => x.IdProvider)
               .OnDelete(DeleteBehavior.Restrict);

        // Configure Category relationship
        builder.HasOne(x => x.Category)
               .WithMany()
               .HasForeignKey(x => x.IdCategory)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ShoppingProducts)
                   .WithOne()
                   .HasForeignKey(x => x.IdShopping);

        builder.HasOne(x => x.ShoppingInventory)
                     .WithOne()
                     .HasForeignKey<InventoryEntity>(x => x.ReferenceId)
                     .OnDelete(DeleteBehavior.Cascade);
    }
}
