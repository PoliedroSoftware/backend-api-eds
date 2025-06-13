using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Shopping.Entities;

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
    }
}